# Sharpener

## What is it?

At the moment, it is a collection of quality-of-life features for C#. It is highly opinionated, although the goal is to be careful and iterative so that these opinionated features will not feel too.. well.. opinionated. A sort of pseudo-empirical if you will. Like enjoying the new Dune movie. Not awfully risky to say, right?

Currently offers:

-   Collection "As" alternatives. So, for example, if you call `ToList()` on a list, it will construct another list! `AsList()` will only provide the new list if it was not one already.
    -   Especially nice if you use Rider/Resharper and it constantly nags you about multiple enumeration (which is nice of it). Now you can address the warning with less chance of using as much memory.
-   Enumerable `ForAll()`, which is like `ForEach()` for lists. It uses `Span<T>`, so the memory implications should be lesser.
-   Syntactically sugarful serialization and deserialization, as well as having the ability to customize the serialization that you get from this logic.
-   Syntactically sugarful case and culture string comparison operations. In case you are tired of littering your code with `StringComparison.OrdinalIgnoreCase` all the time.

## Collections

### AsList and AsArray

Say you are in a method and one of the parameters was an `IEnumerable<string>`.

So it could be anything that inherits from `IEnumerable`, couldn't it? But to avoid multiple enumeration warnings, you want to ensure that you have a list for this context.

```cs
var peopleList = people.ToList()
```

If this was already a list, it'd be nice if this just casted the list as a list, if it was a list.

```cs
var peopleList = people.AsList()
```

This does exactly that.

### LeftJoin

Say that you have two lists and you need to quickly find relationships between the two. For this situation, I've seen a lot of code that looks something like this.

```cs
var studentCandidates = new List<Person>();
foreach(var student in students)
{
    foreach(var candidate in candidates)
    {
        if(student.Name.Equals(candidate.RegisteredName))
        {
            studentCandidates.Add(student);
        }
    }
}
```

Just to find which students are also candidates. Now you could be more succinct..

```cs
var studentCandidates = students
.Where(student => candidates.Any(candidate => student.RegisteredName.Equals(candidate.Name)));
```

But consider that you are iterating through the students list for every entry in the candidates list. That's brutal. Now, not to overlook the value of the `Intersect` and `Union` extensions, there are also join clauses for situations where it's not assumed that equality will be determined by the default equality comparer.

.NET allows you to write code like this, for example

```cs
var studentCandidates = from student in students
join candidate in candidates on student.Name equals candidate.RegisteredName
select student;
```

The performance of the above code is going to be magnitudes of order improved upon the other multiple iteration techniques. Keep that in mind for general knowledge. It's vastly underused.

But not everyone likes the query syntax. If you want this, but in Linq syntax, .Net also offers this.

```cs
var studentCandidates = students.Join(candidates,
student => student.Name,
candidate => candidate.RegisteredName,
(student, candidate) => student);
```

But there's one issue. What if you want a `left join`. A join where, when no equality is found, you still get the value from the first (students) list so that you know that it did not have a match. There are times where you'll need this.
The query syntax offers `into` and `DefaultIfEmpty` for it.

```cs
var leftJoin = from student in students
join candidate in candidates on student.Name equals candidate.RegisteredName into joins
from joined in joins.DefaultIfEmpty()
select new {Student = student, Candidate = joined};

var studentCandidates = leftJoin.Where(pair => pair.Candidate is not null);
```

But to have this in Linq syntax, `Join` doesn't really offer much there. Now, you can use `GroupJoin` to achieve this with Linq, but it's a much more complex lambda structure than one really needs for a simple left join. So now you can simply repeat the previous `Join` sample, but replace it with `LeftJoin` and you'll receive results where the second (candidates) had no match.

```cs
var leftJoin = students.LeftJoin(candidates,
student => student.Name,
candidate => candidate.RegisteredName,
(student, candidate) => new {Student = student, Candidate = candidate});

var studentCandidates = leftJoin.Where(pair => pair.Candidate is not null);
```

## Serialization

### ToJson

To serialize something, you can now just write this.

```cs
var asItemJson = item.ToJson();
```

Want it to stop writing indented? Create a serializer.

```cs
/// <summary>
/// A serializer for System.Text.Json that does not indent.
/// </summary>
public class SystemToRaw : IJsonSerializer
{
    private static JsonSerializerOptions s_options = new JsonSerializerOptions { WriteIndented = false };
    /// <inheritdoc/>
    public Func<object, string> Serialize => model => JsonSerializer.Serialize(model, s_options);
}
```

And now you can either explicitly use it, or register it as the default.

Register as default:

```cs
SharpenerJsonSettings.SetDefaultSerializer<SystemToRaw>();
var asItemJson = item.ToJson();
```

Explicit call, which is better for scenarios where you'd prefer not to change the default and you're accomodating an outlier scenario.

```cs
var asItemJson = item.ToJson<SystemToRaw>();
```

### FromJson

Let's deserialize something. I've got some text that can deserilialize to an `Item`.

```cs
var item = asItemJson.FromJson<Item>();
```

And similar to the `ToJson` options above, you can add a custom deserializer of your own for FromJson as well.

## Strings

This one is a bit opinionated. But, my dudes, I am so sick of writing `StringComparison.SomethingIgnoreCase` all over my code when I don't want to mind case sensitivity. And it's pretty often in my work.

I don't fault the .NET team for this. It was tough to think of something that was logical to clean this mess up, but I have something that I'm fairly content with.

First, let's have a look at a full chain.

```cs
var isEqual = "value".NoCase().Invariant().Equals("Value");
var sameLogic = "value".Equals("Value", StringComparison.InvariantCultureIgnoreCase);
```

Both lines have the same result. The first allocates a few variables as parameters and return values. Otherwise, it's the same.

However, that's as long as this Sharpener syntax gets. There are defaults to use. Are you ok with `StringComparison.OrdinalIgnoreCase`? Cool, let's do this.

```cs
var isEqual = "value".NoCase().Equals("Value");
var sameLogic = "value".Equals("Value", StringComparison.OrdinalIgnoreCase);
```

I like the effect that this has on my code. Plus, I can also do `Contains` work in a mostly similar manner.

```cs
var doesContain = "some-value".NoCase().Contains("Value");
var sameLogic = "some-value".Contains("Value", StringComparison.OrdinalIgnoreCase);
```

While there is also a `Case()` builder option, it's not really needed, since case sensitivity and ordinal culture are the defaults.

```cs
var isEqual = "value".Case().Equals("value");
var sameLogic = "value".Equals("value");
var alsoSameLogic = "value".Equals("value", StringComparison.Ordinal);
```

Want to change the default culture from ordinal?

```cs
SharpenerStringsSettings.SetDefaultCulture(StringComparison.CurrentCulture);
```

This will result in current culture being the default comparison and it will select the case insensitive variant as well.

If you don't want the same culture per case sensitive scenario, you can even use

```cs
SharpenerStringsSettings.TrySetDefaultCultureCaseSensitive(StringComparison.CurrentCulture);
SharpenerStringsSettings.TrySetDefaultCultureCaseInsensitive(StringComparison.InvariantCultureIgnoreCase);
```

Just note that these methods will return false if you try to set a case sensitive culture to the case insensitive method and vice versa.

And in case you missed it in the first example, you have `Invariant()`, `Current()`, and `Ordinal()` builder functions too.

### Links to other Docs

[Roadmap](ROADMAP.MD)

### Thank you

This is my first Open Source repository. Feedback is welcome.

[Ryan Singleton](mailto:ryan@facefire.com)

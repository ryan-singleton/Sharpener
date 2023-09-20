<img src="docs/images/sharpener-logo-40.png"
align="right"
style="height: 40px;" />

# Sharpener

A small scoped and modular ecosystem of packages that intend to provide quality of life improvements for C# developers.
Each package provides these improvements within a domain of work, so that you can keep your references light and
tailored to your needs.

## Table of Contents

- [Sharpener](#sharpener)
  - [Table of Contents](#table-of-contents)
  - [Features](#features)
  - [Installation](#installation)
  - [Usage](#usage)
    - [Concise String Comparisons](#concise-string-comparisons)
    - [Fallback for Strings That Cannot be Null](#fallback-for-strings-that-cannot-be-null)
    - [Collection IsNullOrEmpty](#collection-isnullorempty)
    - [Batch Large Collections](#batch-large-collections)
    - [DistinctBy for Versions Prior to .NET 6](#distinctby-for-versions-prior-to-net-6)
    - [Array Add and Remove](#array-add-and-remove)
    - [ForAll](#forall)
    - [Collection As Extensions](#collection-as-extensions)
    - [LeftJoin](#leftjoin)
  - [REST and HTTP Features](#rest-and-http-features)
  - [Extended Features](#extended-features)
  - [Other Docs](#other-docs)
  - [Support](#support)
  - [Contributing](#contributing)

## Features

The `Sharpener` package focuses on features for the basic runtime layer.

- Concise syntax for string comparisons
  - reducing code clutter due to `StringComparison.SomethingIgnoreCase` spam.
- Array extensions `Add`, `AddRange`, `Remove`, and `RemoveAll`.
  - Faster than converting to `List<T>` in one-off situations for large arrays.
- Fast version of `ForEach` available to all `IEnumerable` types using `Span<T>`.
- A variation from `Join` on lists that allows left joins called `LeftJoin`.
- `As` variants of `ToList` and `ToArray` that will prefer to try casting before creating.
  - If your IDE gives you "multiple enumeration warnings", this one is worth looking at.
      See [Extended Features](#extended-features) for the other extended packages in the Sharpener ecosystem.

## Installation

Navigate to the directory of your .csproj file and then run this command
`dotnet add package Sharpener`
or
`PM> Install-Package Sharpener`

## Usage

### Concise String Comparisons

```cs
var name = "Rick Astley";
var isRickRoll = name.NoCase().StartsWith("rick")
```

Has culture builders as well like `Current()`, `Invariant()`, and `Ordinal()`.

### Fallback for Strings That Cannot be Null

Say you are generating a lookup key based on the values of a type. While some of those values are nullable, like strings, you need every property to generate *something*. You can now use `myString.OrFallback()` and if the string was null or whitespace, you'll end up with "null". If you require a different fallback, you can either use `myString.OrFallback("myFallback")` or you can change the default fallback by changing the `SharpenerStringsSettings.DefaultFallback` value.

### Collection IsNullOrEmpty

For any implementation of `IEnumerable<T>`, you can now call `myCollection.IsNullOrEmpty()` instead of gymnastics like `myCollection?.Any() != true`. It will also check types to be sure that the parameter is not already an array or collection, which should have a flattened `Count` or `Length` value to use already. Failing that, only then does it iterate to check if there are values within.

### Batch Large Collections

It's not uncommon to have the need for breaking a large collection up into a collection of smaller collections. Especially good for work that requires transactions, save points, and so on. You can now use `myCollection.ToBatches(30)` to have a collection of arrays with 30 members.

### DistinctBy for Versions Prior to .NET 6

Since Sharpener is now compatible with .NET Standard 2.0 and above, this makes this highly useful extension available for versions prior to .NET 6. It is not generated for .NET 6 and above, so there should be no conflicts with the built in method when it's available out of the box.

### Array Add and Remove

```cs
itemsArray = itemsArray.Add(newItem);
```

These extensions are a better choice than converting to `List` when the array will be rarely manipulated. Even more so
when the array is large. But when manipulations of the array will be common, conversion to a `List` is more appropriate.
They create a new array using `Span<T>` for high performance, which means that you must re-assign the output to either a
new variable or the original one. This is unlike the `List` operations, where the list is changed by the call.

### ForAll

Say you have an `IEnumerable<Student>`</Student> and you need to reset their `Absences` on all.

```cs
students.ForAll(student => student.Absences = 0);
```

You don't need to cast it as a list and it will run faster with lower memory than if you called `ToList` and
then `ForEach`.

### Collection As Extensions

Say you have that same enumerable from the above example. It has been passed down from method to method and you don't
know, off hand, if it is a list or not.
You perform two operations that may enumerate it twice, so your IDE warns you about this. If it is already a list,
calling `ToList` will make another list from your list. What do you do to settle this warning?

```cs
var studentList = students.AsList();
var aStudents = studentList.Select(student => student.Grade == "A");
var bStudents = studentList.Select(student => student.Grade == "B");
var cStudents = studentList.Select(student => student.Grade == "C");
var dStudents = studentList.Select(student => student.Grade == "D");
var eStudents = studentList.Select(student => student.Grade == "E");
var fStudents = studentList.Select(student => student.Grade == "F");
```

Granted, `GroupBy` is the better option here, but you get the point.

### LeftJoin

Say that you have two lists and you need to quickly find relationships between the two. For this situation, I've seen a
lot of code that looks something like this.

```cs
var studentCandidates = new List<Student>();
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

Just to find which students are also candidates. Now you could be more succinct.

```cs
var studentCandidates = students
.Where(student => candidates.Any(candidate => student.RegisteredName.Equals(candidate.Name)));
```

But consider that you are iterating through the students list for every entry in the candidates list. That's brutal.
Now, not to overlook the value of the `Intersect` and `Union` extensions, there are also join clauses for situations
where it's not assumed that equality will be determined by the default equality comparer.
.NET allows you to write code like this, for example.

```cs
var studentCandidates = from student in students
join candidate in candidates on student.Name equals candidate.RegisteredName
select student;
```

The performance of the above code is going to be magnitudes of order improved upon the other multiple iteration
techniques. Keep that in mind for general knowledge. It's vastly underused.
But not everyone likes the query syntax. If you want this, but in Linq syntax, .Net also offers this.

```cs
var studentCandidates = students.Join(candidates,
student => student.Name,
candidate => candidate.RegisteredName,
(student, candidate) => student);
```

But there's one issue. What if you want a `left join`. A join where, when no equality is found, you still get the value
from the first (students) list so that you know that it did not have a match. There are times where you'll need this.
The query syntax offers `into` and `DefaultIfEmpty` for it.

```cs
var leftJoin = from student in students
join candidate in candidates on student.Name equals candidate.RegisteredName into joins
from joined in joins.DefaultIfEmpty()
select new {Student = student, Candidate = joined};
var studentCandidates = leftJoin.Where(pair => pair.Candidate is not null);
```

But to have this in Linq syntax, `Join` doesn't really offer much there. Now, you can use `GroupJoin` to achieve this
with Linq, but it's a much more complex lambda structure than one really needs for a simple left join. So now you can
simply repeat the previous `Join` sample, but replace it with `LeftJoin` and you'll receive results where the second (
candidates) had no match.

```cs
var leftJoin = students.LeftJoin(candidates,
student => student.Name,
candidate => candidate.RegisteredName,
(student, candidate) => new {Student = student, Candidate = candidate});
var studentCandidates = leftJoin.Where(pair => pair.Candidate is not null);
```

## REST and HTTP Features

The baked-in tooling for writing web requests can be ugly, verbose, and difficult to test. This is why there are many
libraries like [RestSharp](https://restsharp.dev/) and [Flurl](https://flurl.dev/) that offer new and more manageable
ways to write requests. Influenced by these, but with intent to keep our footprint and framework shallow, so that you
are staying closer to the fundamental .NET level, this library offers you a simple and basic set of features for fluent
request syntax.

```cs
var result = await httpClient.Rest("https://nice-service.com/api")
    .SetPaths("accounts", "create")
    .SetBearerToken(token)
    .AddQueries(new
    {
        Expired = false,
        Owner = true,
    })
    .SetJsonContent(createAccount)
    .PostAsync()
    .ReadJsonAs<CreatedAccount>();

var createdAccount = result.IsSuccess
    ? result.Value
    : // handle failures
```

## Extended Features

- [Json](docs/JSON.md)
  - [Newtonsoft](docs/Newtonsoft.md)
- [REST](docs/REST.md)

## Other Docs

- [Contributing](docs/CONTRIBUTING.md)
- [Changelog](docs/CHANGELOG.md)
- [Benchmarks](docs/BENCHMARKS.md)
- [Roadmap](docs/ROADMAP.MD)

## Support

Please [open an issue](https://github.com/ryan-singleton/Sharpener/issues/new) for support.

## Contributing

Please contribute using [Github Flow](https://guides.github.com/introduction/flow/). Create a branch, add commits,
and [open a pull request](https://github.com/ryan-singleton/Sharpener/compare/).

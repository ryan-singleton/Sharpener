<img src="docs/images/icon-white-stroke-40px.png"
     align="right"
     style="height: 40px;" />

# Sharpener

A small scoped and modular ecosystem of packages that intend to provide quality of life improvements for C# developers. Each package provides these improvements within a domain of work, so that you can keep your references light and tailored to your needs.

## Table of Contents

- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
- [Extended Features](#extended-features)
- [Other Docs](#other-docs)
- [Support](#support)
- [Contributing](#contributing)

## Features

The `Sharpener` package focuses on features for the basic runtime layer.

- Concise syntax for string comparisons
  - reducing code clutter due to `StringComparison.SomethingIgnoreCase` spam.
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

### ForAll

Say you have an `IEnumerable<Student>`</Student> and you need to reset their `Absences` on all.

```cs
students.ForAll(student => student.Absences = 0);
```

You don't need to cast it as a list and it will run faster with lower memory than if you called `ToList` and then `ForEach`.

### Collection As Extensions

Say you have that same enumerable from the above example. It has been passed down from method to method and you don't know, off hand, if it is a list or not.
You perform two operations that may enumerate it twice, so your IDE warns you about this. If it is already a list, calling `ToList` will make another list from your list. What do you do to settle this warning?

```cs
var studentList = students.AsList();
var bStudents = studentList.Select(student => student.Grade == "A");
var bStudents = studentList.Select(student => student.Grade == "B");
var cStudents = studentList.Select(student => student.Grade == "C");
var bStudents = studentList.Select(student => student.Grade == "D");
var cStudents = studentList.Select(student => student.Grade == "E");
var bStudents = studentList.Select(student => student.Grade == "F");
```

Granted, `GroupBy` is the better option here, but you get the point.

### LeftJoin

Say that you have two lists and you need to quickly find relationships between the two. For this situation, I've seen a lot of code that looks something like this.

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

But consider that you are iterating through the students list for every entry in the candidates list. That's brutal. Now, not to overlook the value of the `Intersect` and `Union` extensions, there are also join clauses for situations where it's not assumed that equality will be determined by the default equality comparer.
.NET allows you to write code like this, for example.

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

## Extended Features

- [Json](docs/JSON.md)
  - [Newtonsoft](docs/Newtonsoft.md)
- [Http](docs/Http.md)

## Other Docs

- [Contributing](docs/CONTRIBUTING.md)
- [Changelog](docs/CHANGELOG.md)
- [Benchmarks](docs/BENCHMARKS.md)
- [Roadmap](docs/ROADMAP.MD)

## Support

Please [open an issue](https://github.com/r-singleton/Sharpener/issues/new) for support.

## Contributing

Please contribute using [Github Flow](https://guides.github.com/introduction/flow/). Create a branch, add commits, and [open a pull request](https://github.com/r-singleton/Sharpener/compare/).

using AutoBogus;
using BenchmarkDotNet.Attributes;
using Sharpener.Benchmarks.Models;
using Sharpener.Extensions;

namespace Sharpener.Benchmarks.Extensions;

public class StringExtensionsTests
{
    [Params(100, 100_000, 1_000_000)]
    public int Count;
    private readonly AutoFaker<Item> _autoFaker = default!;
    private readonly List<Item> _itemsList = default!;
    private readonly Item[] _itemsArray = default!;

    private const string Source = "value";

    private const string Compare = "Value";

    [GlobalSetup]
    public void Setup()
    {

    }

    [Benchmark]
    public void Equals_Original() => Source.Equals(Compare);

    [Benchmark]
    public void Case_Equivalent() => Source.Equals(Compare, StringComparison.Ordinal);

    [Benchmark]
    public void Case() => Source.Case().Equals(Compare);

    [Benchmark]
    public void NoCase_Equivalent() => Source.Equals(Compare, StringComparison.OrdinalIgnoreCase);

    [Benchmark]
    public void NoCase() => Source.NoCase().Equals(Compare);

    [Benchmark]
    public void NoCase_Current_Equivalent() => Source.Equals(Compare, StringComparison.CurrentCultureIgnoreCase);

    [Benchmark]
    public void NoCase_Current() => Source.NoCase().Current().Equals(Compare);

    [Benchmark]
    public void Current_NoCase() => Source.Current().NoCase().Equals(Compare);
}

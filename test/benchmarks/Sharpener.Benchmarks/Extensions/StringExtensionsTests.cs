// The Sharpener project licenses this file to you under the MIT license.

namespace Sharpener.Benchmarks.Extensions;

using BenchmarkDotNet.Attributes;
using Sharpener.Extensions;

[MemoryDiagnoser(false)]
public class StringExtensionsTests
{
    private readonly string Compare = "Value";
    private readonly string Source = "value";

    [Params(100, 100_000, 1_000_000)] public int Count;

    [Benchmark(Description = "string.Case().Equals(string)")]
    public void Case()
    {
        Source.Case().Equals(Compare);
    }

    [Benchmark(Description = "string.Equals(string, StringComparison.Ordinal)")]
    public void Case_Equivalent()
    {
        Source.Equals(Compare, StringComparison.Ordinal);
    }

    [Benchmark(Description = "string.Current().NoCase().Equals(string)")]
    public void Current_NoCase()
    {
        Source.Current().NoCase().Equals(Compare);
    }

    [Benchmark(Description = "string.Equals(string)")]
    public void Equals_Original()
    {
        Source.Equals(Compare);
    }

    [Benchmark(Description = "string.NoCase().Equals(string)")]
    public void NoCase()
    {
        Source.NoCase().Equals(Compare);
    }

    [Benchmark(Description = "string.NoCase().Current().Equals(string)")]
    public void NoCase_Current()
    {
        Source.NoCase().Current().Equals(Compare);
    }

    [Benchmark(Description = "string.Equals(string, StringComparison.CurrentCultureIgnoreCase)")]
    public void NoCase_Current_Equivalent()
    {
        Source.Equals(Compare, StringComparison.CurrentCultureIgnoreCase);
    }

    [Benchmark(Description = "string.Equals(string, StringComparison.OrdinalIgnoreCase)")]
    public void NoCase_Equivalent()
    {
        Source.Equals(Compare, StringComparison.OrdinalIgnoreCase);
    }

    [GlobalSetup]
    public static void Setup()
    {
    }
}

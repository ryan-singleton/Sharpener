using BenchmarkDotNet.Running;
using Sharpener.Benchmarks.Extensions;

namespace Sharpener.Benchmarks;

public class Program
{
    public static void Main()
    {
        _ = BenchmarkRunner.Run<CollectionExtensionsTests>();
        _ = BenchmarkRunner.Run<StringExtensionsTests>();
    }
}

using BenchmarkDotNet.Running;
using Sharpener.Benchmarks.Extensions;

namespace Sharpener.Benchmarks;

public class Program
{
    public static void Main() => BenchmarkRunner.Run<CollectionExtensionsTests>();
}

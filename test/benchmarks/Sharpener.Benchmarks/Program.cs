// The Sharpener project licenses this file to you under the MIT license.

namespace Sharpener.Benchmarks;

using BenchmarkDotNet.Running;
using Extensions;

public class Program
{
    public static void Main()
    {
        BenchmarkRunner.Run<CollectionExtensionsTests>();
        BenchmarkRunner.Run<StringExtensionsTests>();
    }
}

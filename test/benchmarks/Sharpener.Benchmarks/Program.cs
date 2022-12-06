// The Sharpener project and Facefire license this file to you under the MIT license.

using BenchmarkDotNet.Running;
using Sharpener.Benchmarks.Extensions;
namespace Sharpener.Benchmarks;
public class Program
{
    public static void Main()
    {
        BenchmarkRunner.Run<CollectionExtensionsTests>();
        BenchmarkRunner.Run<StringExtensionsTests>();
    }
}

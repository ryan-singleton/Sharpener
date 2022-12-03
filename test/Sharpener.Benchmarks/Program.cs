using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Sharpener.Benchmarks.Extensions;

namespace Sharpener.Benchmarks;

public class Program
{
    private const string LogPath = @"/benchmark-logs";
    public static void Main()
    {
        BenchmarkRunner.Run<CollectionExtensionsTests>(ManualConfig.Create(DefaultConfig.Instance).WithArtifactsPath(LogPath));
        BenchmarkRunner.Run<StringExtensionsTests>(ManualConfig.Create(DefaultConfig.Instance).WithArtifactsPath(LogPath)); ;
    }
}

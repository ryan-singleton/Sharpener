using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Sharpener.Benchmarks.Extensions;

namespace Sharpener.Benchmarks;

public class Program
{
    private const string LogPath = @"/benchmark-logs";
    public static void Main()
    {
        _ = BenchmarkRunner.Run<CollectionExtensionsTests>(ManualConfig.Create(DefaultConfig.Instance).WithArtifactsPath(LogPath));
        _ = BenchmarkRunner.Run<StringExtensionsTests>(ManualConfig.Create(DefaultConfig.Instance).WithArtifactsPath(LogPath)); ;
    }
}

using AutoBogus;
using BenchmarkDotNet.Attributes;
using Sharpener.Benchmarks.Models;
using Sharpener.Extensions;

namespace Sharpener.Benchmarks.Extensions;

[MemoryDiagnoser(false)]
public class CollectionExtensionsTests
{
    [Params(100, 100_000, 1_000_000)]
    public int Count;
    private AutoFaker<Item> _autoFaker = default!;
    private List<Item> _itemsList = default!;
    private Item[] _itemsArray = default!;

    private const string Name = "Test";

    [GlobalSetup]
    public void Setup()
    {
        _autoFaker = new AutoFaker<Item>();

        _itemsList = _autoFaker.Generate(Count);
        _itemsArray = _itemsList.ToArray();
    }

    [Benchmark(Description = "list.ForAll()")]
    public void ForAll() => _itemsList.ForAll(item => item.Name = Name);

    [Benchmark(Description = "list.ForEach()")]
    public void ForEach() => _itemsList.ForEach(item => item.Name = Name);

    [Benchmark(Description = "foreach in list")]
    public void ForEachManual()
    {
        foreach (Item item in _itemsList)
        {
            item.Name = Name;
        }
    }

    [Benchmark(Description = "for in list")]
    public void ForManual()
    {
        for (int i = 0; i < _itemsList.Count; i++)
        {
            _itemsList[i].Name = Name;
        }
    }

    [Benchmark(Description = "list.ToList()")]
    public void ToList_WhenWasList() => _itemsList.ToList();

    [Benchmark(Description = "list.AsList()")]
    public void AsList_WhenWasList() => _itemsList.AsList();

    [Benchmark(Description = "array.ToList()")]
    public void ToList() => _itemsArray.ToList();

    [Benchmark(Description = "array.AsList()")]
    public void AsList() => _itemsArray.AsList();

    [Benchmark(Description = "list.ToArray()")]
    public void ToArray() => _itemsList.ToArray();

    [Benchmark(Description = "list.AsArray()")]
    public void AsArray() => _itemsList.AsArray();

    [Benchmark(Description = "array.ToArray()")]
    public void ToArray_WhenWasArray() => _itemsArray.ToArray();

    [Benchmark(Description = "array.AsArray()")]
    public void AsArray_WhenWasArray() => _itemsArray.AsArray();
}

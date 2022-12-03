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

    [Benchmark]
    public void ForAll() => _itemsList.ForAll(item => item.Name = Name);

    [Benchmark]
    public void ForEach() => _itemsList.ForEach(item => item.Name = Name);

    [Benchmark]
    public void ForEachManual()
    {
        foreach (var item in _itemsList)
        {
            item.Name = Name;
        }
    }

    [Benchmark]
    public void ForManual()
    {
        for (var i = 0; i < _itemsList.Count; i++)
        {
            _itemsList[i].Name = Name;
        }
    }

    [Benchmark]
    public void ToList_WhenWasList() => _itemsList.ToList();

    [Benchmark]
    public void AsList_WhenWasList() => _itemsList.AsList();

    [Benchmark]
    public void ToList() => _itemsArray.ToList();

    [Benchmark]
    public void AsList() => _itemsArray.AsList();

    [Benchmark]
    public void ToArray() => _itemsList.ToArray();

    [Benchmark]
    public void AsArray() => _itemsList.AsArray();

    [Benchmark]
    public void ToArray_WhenWasArray() => _itemsArray.ToArray();

    [Benchmark]
    public void AsArray_WhenWasArray() => _itemsArray.AsArray();
}

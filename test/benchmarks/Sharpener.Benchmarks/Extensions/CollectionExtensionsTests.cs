// The Sharpener project licenses this file to you under the MIT license.

using AutoBogus;
using BenchmarkDotNet.Attributes;
using Sharpener.Extensions;
using Sharpener.Tests.Common.Models;

namespace Sharpener.Benchmarks.Extensions;

[MemoryDiagnoser(false)]
public class CollectionExtensionsTests
{
    private const string Name = "Test";
    private AutoFaker<Item> _autoFaker = default!;
    private Item _item = default!;
    private Item[] _itemsArray = default!;
    private List<Item> _itemsList = default!;
    private Item[] _secondItemsArray = default!;
    private List<Item> _secondItemsList = default!;

    [Params(100, 100_000, 1_000_000)] public int Count;

    [Benchmark(Description = "array.Add(member)")]
    public void Array_Add()
    {
        _itemsArray.Add(_item);
    }

    [Benchmark(Description = "array.AddRange(membersArray)")]
    public void Array_AddRange()
    {
        _itemsArray.AddRange(_secondItemsArray);
    }

    [Benchmark(Description = "array.Remove(member)")]
    public void Array_Remove()
    {
        _itemsArray.Remove(_itemsArray[2]);
    }

    [Benchmark(Description = "array.RemoveAll(condition)")]
    public void Array_RemoveAll()
    {
        _itemsArray.RemoveAll(x => x.Equals(_itemsArray[2]));
    }

    [Benchmark(Description = "array.ToList().Add(member)")]
    public void Array_ToList_Add()
    {
        _itemsArray.ToList().Add(_item);
    }

    [Benchmark(Description = "array.ToList().AddRange(membersArray)")]
    public void Array_ToList_AddRange()
    {
        _itemsArray.ToList().AddRange(_secondItemsArray);
    }

    [Benchmark(Description = "array.ToList().Remove(member)")]
    public void Array_ToList_Remove()
    {
        _itemsArray.ToList().Remove(_itemsArray[2]);
    }

    [Benchmark(Description = "array.ToList().RemoveAll(condition)")]
    public void Array_ToList_RemoveAll()
    {
        _itemsArray.ToList().RemoveAll(x => x.Equals(_itemsArray[2]));
    }

    [Benchmark(Description = "list.AsArray()")]
    public void AsArray()
    {
        _itemsList.AsArray();
    }

    [Benchmark(Description = "array.AsArray()")]
    public void AsArray_WhenWasArray()
    {
        _itemsArray.AsArray();
    }

    [Benchmark(Description = "array.AsList()")]
    public void AsList()
    {
        _itemsArray.AsList();
    }

    [Benchmark(Description = "list.AsList()")]
    public void AsList_WhenWasList()
    {
        _itemsList.AsList();
    }

    [Benchmark(Description = "list.ForAll()")]
    public void ForAll()
    {
        _itemsList.ForAll(item => item.Name = Name);
    }

    [Benchmark(Description = "list.ForEach()")]
    public void ForEach()
    {
        _itemsList.ForEach(item => item.Name = Name);
    }

    [Benchmark(Description = "foreach in list")]
    public void ForEachManual()
    {
        foreach (var item in _itemsList)
        {
            item.Name = Name;
        }
    }

    [Benchmark(Description = "for in list")]
    public void ForManual()
    {
        for (var i = 0; i < _itemsList.Count; i++)
        {
            _itemsList[i].Name = Name;
        }
    }

    [GlobalSetup]
    public void Setup()
    {
        _autoFaker = new AutoFaker<Item>();
        _itemsList = _autoFaker.Generate(Count);
        _secondItemsList = _autoFaker.Generate(20);
        _itemsArray = _itemsList.ToArray();
        _secondItemsArray = _secondItemsList.ToArray();
        _item = _autoFaker.Generate();
    }

    [Benchmark(Description = "list.ToArray()")]
    public void ToArray()
    {
        _itemsList.ToArray();
    }

    [Benchmark(Description = "array.ToArray()")]
    public void ToArray_WhenWasArray()
    {
        _itemsArray.ToArray();
    }

    [Benchmark(Description = "array.ToList()")]
    public void ToList()
    {
        _itemsArray.ToList();
    }

    [Benchmark(Description = "list.ToList()")]
    public void ToList_WhenWasList()
    {
        _itemsList.ToList();
    }
}

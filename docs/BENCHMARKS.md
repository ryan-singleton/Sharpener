## Benchmarks

These are the most recent benchmarks to help in optimizing this library.

### String Comparison

```ini
BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22621.819)
Intel Core i9-9900K CPU 3.60GHz (Coffee Lake), 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.401
  [Host]     : .NET 6.0.9 (6.0.922.41905), X64 RyuJIT AVX2
  DefaultJob : .NET 6.0.9 (6.0.922.41905), X64 RyuJIT AVX2
```

| Method                                                           | Count   |      Mean |     Error |    StdDev | Allocated |
| ---------------------------------------------------------------- | ------- | --------: | --------: | --------: | --------: |
| string.Equals(string)                                            | 100     |  2.695 ns | 0.0098 ns | 0.0082 ns |         - |
| string.Equals(string, StringComparison.Ordinal)                  | 100     |  4.670 ns | 0.0113 ns | 0.0100 ns |         - |
| string.Case().Equals(string)                                     | 100     |  4.708 ns | 0.0146 ns | 0.0130 ns |         - |
| string.Equals(string, StringComparison.OrdinalIgnoreCase)        | 100     |  5.878 ns | 0.0177 ns | 0.0156 ns |         - |
| string.NoCase().Equals(string)                                   | 100     |  5.744 ns | 0.0138 ns | 0.0123 ns |         - |
| string.Equals(string, StringComparison.CurrentCultureIgnoreCase) | 100     | 74.415 ns | 0.1689 ns | 0.1411 ns |         - |
| string.NoCase().Current().Equals(string)                         | 100     | 83.319 ns | 0.1790 ns | 0.1495 ns |      32 B |
| string.Current().NoCase().Equals(string)                         | 100     | 84.264 ns | 0.1062 ns | 0.0941 ns |      32 B |
| string.Equals(string)                                            | 100000  |  2.642 ns | 0.0067 ns | 0.0060 ns |         - |
| string.Equals(string, StringComparison.Ordinal)                  | 100000  |  4.656 ns | 0.0163 ns | 0.0145 ns |         - |
| string.Case().Equals(string)                                     | 100000  |  4.921 ns | 0.0136 ns | 0.0127 ns |         - |
| string.Equals(string, StringComparison.OrdinalIgnoreCase)        | 100000  |  5.880 ns | 0.0529 ns | 0.0441 ns |         - |
| string.NoCase().Equals(string)                                   | 100000  |  6.342 ns | 0.0812 ns | 0.0678 ns |         - |
| string.Equals(string, StringComparison.CurrentCultureIgnoreCase) | 100000  | 71.148 ns | 0.0993 ns | 0.0881 ns |         - |
| string.NoCase().Current().Equals(string)                         | 100000  | 81.430 ns | 0.3548 ns | 0.3319 ns |      32 B |
| string.Current().NoCase().Equals(string)                         | 100000  | 82.724 ns | 1.1497 ns | 0.9600 ns |      32 B |
| string.Equals(string)                                            | 1000000 |  2.537 ns | 0.0242 ns | 0.0226 ns |         - |
| string.Equals(string, StringComparison.Ordinal)                  | 1000000 |  4.464 ns | 0.0330 ns | 0.0293 ns |         - |
| string.Case().Equals(string)                                     | 1000000 |  4.712 ns | 0.0347 ns | 0.0324 ns |         - |
| string.Equals(string, StringComparison.OrdinalIgnoreCase)        | 1000000 |  5.601 ns | 0.0298 ns | 0.0249 ns |         - |
| string.NoCase().Equals(string)                                   | 1000000 |  5.484 ns | 0.0380 ns | 0.0355 ns |         - |
| string.Equals(string, StringComparison.CurrentCultureIgnoreCase) | 1000000 | 70.381 ns | 0.4980 ns | 0.4658 ns |         - |
| string.NoCase().Current().Equals(string)                         | 1000000 | 82.321 ns | 0.3182 ns | 0.2657 ns |      32 B |
| string.Current().NoCase().Equals(string)                         | 1000000 | 82.013 ns | 0.4640 ns | 0.4340 ns |      32 B |

Conclusions

- The instantiated structs only occur with culture builders and don't matter much. It's 32 B no matter the amount in the list.
- Two to ten nanosecond increases, no matter the size of the collection, on evaluation.
- It would be good if they can eventually offer performance and memory boosts, but for now, they have insignificantly small impacts.

### Collection Extensions

```ini
BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22621.819)
Intel Core i9-9900K CPU 3.60GHz (Coffee Lake), 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.401
  [Host]     : .NET 6.0.9 (6.0.922.41905), X64 RyuJIT AVX2
  DefaultJob : .NET 6.0.9 (6.0.922.41905), X64 RyuJIT AVX2
```

| Method                              | Count   |               Mean |           Error |          StdDev |  Allocated |
| ----------------------------------- | ------- | -----------------: | --------------: | --------------: | ---------: |
| list.ForAll()                       | 100     |        282.4624 ns |       2.8132 ns |       2.4939 ns |          - |
| list.ForEach()                      | 100     |        316.8117 ns |       0.2893 ns |       0.2259 ns |          - |
| foreach in list                     | 100     |        198.1339 ns |       0.3937 ns |       0.3682 ns |          - |
| for in list                         | 100     |        159.0870 ns |       0.2858 ns |       0.2231 ns |          - |
| list.ToList()                       | 100     |         64.3185 ns |       0.4487 ns |       0.4197 ns |      856 B |
| list.AsList()                       | 100     |          0.4304 ns |       0.0049 ns |       0.0043 ns |          - |
| array.ToList()                      | 100     |         74.7194 ns |       1.3879 ns |       1.2303 ns |      856 B |
| array.AsList()                      | 100     |         76.0893 ns |       0.6528 ns |       0.6411 ns |      856 B |
| list.ToArray()                      | 100     |         50.9780 ns |       0.9335 ns |       0.8275 ns |      824 B |
| list.AsArray()                      | 100     |         60.2152 ns |       0.2484 ns |       0.2075 ns |      824 B |
| array.ToArray()                     | 100     |         68.6360 ns |       0.7498 ns |       0.6647 ns |      824 B |
| array.AsArray()                     | 100     |          0.2155 ns |       0.0033 ns |       0.0031 ns |          - |
| array.Add(member)                   | 100     |         47.7517 ns |       0.3438 ns |       0.2684 ns |      832 B |
| array.ToList().Add(member)          | 100     |        163.1836 ns |       2.5315 ns |       2.3680 ns |     2480 B |
| array.AddRange(membersArray)        | 100     |         97.9960 ns |       1.4993 ns |       1.3291 ns |      984 B |
| array.ToList().Add(membersArray)    | 100     |        183.7379 ns |       0.7945 ns |       0.7043 ns |     2480 B |
| array.Remove(member)                | 100     |        100.6419 ns |       0.7025 ns |       0.5866 ns |      816 B |
| array.ToList().Remove(member)       | 100     |        113.4565 ns |       0.9598 ns |       0.8978 ns |      856 B |
| array.RemoveAll(condition)          | 100     |        296.4313 ns |       1.1950 ns |       1.0593 ns |      104 B |
| array.ToList().RemoveAll(condition) | 100     |        718.3564 ns |       4.0215 ns |       3.3581 ns |      920 B |
| list.ForAll()                       | 100000  |    286,684.0849 ns |   5,685.7944 ns |  15,274.5173 ns |          - |
| list.ForEach()                      | 100000  |    325,002.2746 ns |   1,499.2105 ns |   1,170.4849 ns |          - |
| foreach in list                     | 100000  |    232,289.3341 ns |   4,380.2059 ns |   4,301.9479 ns |          - |
| for in list                         | 100000  |    231,067.1997 ns |   4,539.3603 ns |   5,902.4506 ns |          - |
| list.ToList()                       | 100000  |    262,747.7539 ns |   4,844.8493 ns |   4,531.8750 ns |   800077 B |
| list.AsList()                       | 100000  |          0.4286 ns |       0.0011 ns |       0.0009 ns |          - |
| array.ToList()                      | 100000  |    268,222.4243 ns |   5,173.3886 ns |   6,726.8665 ns |   800078 B |
| array.AsList()                      | 100000  |    268,367.3160 ns |   5,353.2960 ns |   5,950.1743 ns |   800077 B |
| list.ToArray()                      | 100000  |    250,256.9420 ns |   4,928.7829 ns |  10,818.7980 ns |   800017 B |
| list.AsArray()                      | 100000  |    252,843.3075 ns |   4,770.7458 ns |   9,416.9867 ns |   800010 B |
| array.ToArray()                     | 100000  |    254,559.6915 ns |   5,079.3927 ns |  13,557.9275 ns |   800017 B |
| array.AsArray()                     | 100000  |          0.4343 ns |       0.0045 ns |       0.0040 ns |          - |
| array.Add(member)                   | 100000  |    253,843.9277 ns |   5,048.8264 ns |  14,404.5857 ns |   800025 B |
| array.ToList().Add(member)          | 100000  |    564,938.4820 ns |  11,113.9041 ns |  15,580.1414 ns |  2400163 B |
| array.AddRange(membersArray)        | 100000  |    255,569.4627 ns |   5,064.3542 ns |  13,605.0586 ns |   800178 B |
| array.ToList().Add(membersArray)    | 100000  |    565,716.2184 ns |  10,415.6263 ns |   8,697.5197 ns |  2400149 B |
| array.Remove(member)                | 100000  |    256,028.6536 ns |   4,939.4524 ns |   6,246.8257 ns |   800008 B |
| array.ToList().Remove(member)       | 100000  |    293,324.7001 ns |   4,110.1967 ns |   3,643.5823 ns |   800078 B |
| array.RemoveAll(condition)          | 100000  |    718,188.6914 ns |  60,354.6586 ns | 177,957.0725 ns |      105 B |
| array.ToList().RemoveAll(condition) | 100000  |  1,037,514.4857 ns |  20,170.8535 ns |  21,582.5873 ns |   800434 B |
| list.ForAll()                       | 1000000 |  6,286,558.5156 ns | 123,060.0466 ns | 115,110.4439 ns |        7 B |
| list.ForEach()                      | 1000000 |  6,267,922.4888 ns |  97,172.4881 ns |  86,140.8797 ns |        7 B |
| foreach in list                     | 1000000 |  6,253,316.0677 ns |  97,992.3617 ns |  91,662.1160 ns |        7 B |
| for in list                         | 1000000 |  6,196,562.7232 ns |  77,695.2439 ns |  68,874.8100 ns |        7 B |
| list.ToList()                       | 1000000 |  5,327,887.6042 ns | 102,824.6808 ns |  96,182.2702 ns |  8000110 B |
| list.AsList()                       | 1000000 |          0.2116 ns |       0.0053 ns |       0.0047 ns |          - |
| array.ToList()                      | 1000000 |  5,282,575.8333 ns |  91,556.1431 ns |  85,641.6731 ns |  8000110 B |
| array.AsList()                      | 1000000 |  5,232,088.2031 ns |  70,707.0341 ns |  66,139.4036 ns |  8000110 B |
| list.ToArray()                      | 1000000 |  5,264,212.1354 ns |  84,570.2837 ns |  79,107.0959 ns |  8000078 B |
| list.AsArray()                      | 1000000 |  5,296,209.9524 ns | 105,885.5350 ns | 133,911.2986 ns |  8000078 B |
| array.ToArray()                     | 1000000 |  5,244,830.4167 ns | 104,838.4908 ns |  98,065.9894 ns |  8000078 B |
| array.AsArray()                     | 1000000 |          0.2197 ns |       0.0053 ns |       0.0047 ns |          - |
| array.Add(member)                   | 1000000 |  5,312,529.0104 ns |  94,310.5638 ns |  88,218.1599 ns |  8000086 B |
| array.ToList().Add(member)          | 1000000 |  6,431,003.8206 ns | 128,259.3091 ns | 319,410.1878 ns | 24000481 B |
| array.AddRange(membersArray)        | 1000000 |  5,221,626.6276 ns |  64,778.0142 ns |  50,574.4126 ns |  8000238 B |
| array.ToList().Add(membersArray)    | 1000000 |  6,214,308.5399 ns | 123,694.0802 ns | 181,309.1789 ns | 24000610 B |
| array.Remove(member)                | 1000000 |  5,260,015.9180 ns |  97,605.0742 ns |  95,861.2338 ns |  8000070 B |
| array.ToList().Remove(member)       | 1000000 |  6,054,426.8229 ns |  55,383.0305 ns |  51,805.3211 ns |  8000110 B |
| array.RemoveAll(condition)          | 1000000 | 14,479,129.2411 ns | 227,541.0635 ns | 201,709.2260 ns |  8000135 B |
| array.ToList().RemoveAll(condition) | 1000000 | 14,731,266.4583 ns |  67,367.1366 ns |  63,015.2614 ns |  8000175 B |

Conclusions

- Array `Add` extensions become less useful after 3 calls.
  - Small sized arrays run 3x faster and with 3x less memory.
  - Medium sized arrays run 2x+ faster and with 3x less memory.
  - Large sized arrays run at about the same speed but 3x less memory.
  - After 3 calls, the cost of creating a new list is surpassed for using lists.
- Array `Remove` extension breaks even typically. Suggest sticking to 3x or less rule.
- Array `RemoveAll` is nuts. Shows incredible gains in memory and speed except at large size, where it suddenly breaks even.
  - Needs more investigation.
- `ForAll` on a small list is comparable to `ForEach`.
- `ForAll` on a large list scales to compare to the faster `foreach` and `for`.
  - So you may want to use `ForAll` over `ForEach` on lists due to a similar floor and much higher ceiling.
- `AsArray` and `AsList` are extremely valuable when the collection was already an array or list respectively.
  - Both in performance speed and memory allocation.

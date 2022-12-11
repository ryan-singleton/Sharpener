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
| string.Equals(string)                                            | 100     |  2.643 ns | 0.0147 ns | 0.0138 ns |         - |
| string.Equals(string, StringComparison.Ordinal)                  | 100     |  4.556 ns | 0.0215 ns | 0.0201 ns |         - |
| string.Case().Equals(string)                                     | 100     |  4.822 ns | 0.0183 ns | 0.0171 ns |         - |
| string.Equals(string, StringComparison.OrdinalIgnoreCase)        | 100     |  5.692 ns | 0.0139 ns | 0.0130 ns |         - |
| string.NoCase().Equals(string)                                   | 100     |  5.646 ns | 0.0195 ns | 0.0182 ns |         - |
| string.Equals(string, StringComparison.CurrentCultureIgnoreCase) | 100     | 71.116 ns | 0.1701 ns | 0.1420 ns |         - |
| string.NoCase().Current().Equals(string)                         | 100     | 82.114 ns | 0.1825 ns | 0.1524 ns |      32 B |
| string.Current().NoCase().Equals(string)                         | 100     | 82.182 ns | 0.5173 ns | 0.4839 ns |      32 B |
| string.Equals(string)                                            | 100000  |  2.600 ns | 0.0118 ns | 0.0110 ns |         - |
| string.Equals(string, StringComparison.Ordinal)                  | 100000  |  4.580 ns | 0.0130 ns | 0.0116 ns |         - |
| string.Case().Equals(string)                                     | 100000  |  4.676 ns | 0.0780 ns | 0.0729 ns |         - |
| string.Equals(string, StringComparison.OrdinalIgnoreCase)        | 100000  |  5.835 ns | 0.0321 ns | 0.0300 ns |         - |
| string.NoCase().Equals(string)                                   | 100000  |  5.691 ns | 0.0254 ns | 0.0238 ns |         - |
| string.Equals(string, StringComparison.CurrentCultureIgnoreCase) | 100000  | 72.109 ns | 0.2148 ns | 0.2009 ns |         - |
| string.NoCase().Current().Equals(string)                         | 100000  | 80.602 ns | 0.5452 ns | 0.5100 ns |      32 B |
| string.Current().NoCase().Equals(string)                         | 100000  | 77.569 ns | 0.6179 ns | 0.5159 ns |      32 B |
| string.Equals(string)                                            | 1000000 |  2.180 ns | 0.0091 ns | 0.0080 ns |         - |
| string.Equals(string, StringComparison.Ordinal)                  | 1000000 |  4.658 ns | 0.0131 ns | 0.0122 ns |         - |
| string.Case().Equals(string)                                     | 1000000 |  4.674 ns | 0.0586 ns | 0.0548 ns |         - |
| string.Equals(string, StringComparison.OrdinalIgnoreCase)        | 1000000 |  5.839 ns | 0.0894 ns | 0.0837 ns |         - |
| string.NoCase().Equals(string)                                   | 1000000 |  5.735 ns | 0.0642 ns | 0.0501 ns |         - |
| string.Equals(string, StringComparison.CurrentCultureIgnoreCase) | 1000000 | 73.278 ns | 0.2155 ns | 0.1910 ns |         - |
| string.NoCase().Current().Equals(string)                         | 1000000 | 81.068 ns | 0.1345 ns | 0.1123 ns |      32 B |
| string.Current().NoCase().Equals(string)                         | 1000000 | 80.128 ns | 0.5964 ns | 0.4980 ns |      32 B |

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

| Method                                | Count   |               Mean |           Error |          StdDev |             Median |  Allocated |
| ------------------------------------- | ------- | -----------------: | --------------: | --------------: | -----------------: | ---------: |
| list.ForAll()                         | 100     |        287.1548 ns |       4.8526 ns |       6.3098 ns |        284.8127 ns |          - |
| list.ForEach()                        | 100     |        284.7799 ns |       0.3207 ns |       0.2504 ns |        284.6804 ns |          - |
| foreach in list                       | 100     |        200.4375 ns |       3.4061 ns |       3.0195 ns |        199.0455 ns |          - |
| for in list                           | 100     |        159.0303 ns |       0.2544 ns |       0.2380 ns |        159.1357 ns |          - |
| list.ToList()                         | 100     |         65.2797 ns |       1.0203 ns |       0.8520 ns |         65.2465 ns |      856 B |
| list.AsList()                         | 100     |          0.4294 ns |       0.0066 ns |       0.0059 ns |          0.4264 ns |          - |
| array.ToList()                        | 100     |         74.2299 ns |       1.1057 ns |       0.9802 ns |         74.2885 ns |      856 B |
| array.AsList()                        | 100     |         72.3096 ns |       0.8228 ns |       0.7697 ns |         72.3512 ns |      856 B |
| list.ToArray()                        | 100     |         49.4659 ns |       0.9716 ns |       1.0396 ns |         49.7126 ns |      824 B |
| list.AsArray()                        | 100     |         59.4056 ns |       0.5640 ns |       0.5276 ns |         59.5047 ns |      824 B |
| array.ToArray()                       | 100     |         66.0091 ns |       0.8259 ns |       0.6896 ns |         66.2358 ns |      824 B |
| array.AsArray()                       | 100     |          0.2157 ns |       0.0014 ns |       0.0011 ns |          0.2159 ns |          - |
| array.Add(member)                     | 100     |         55.7403 ns |       1.0975 ns |       1.0266 ns |         55.1892 ns |      832 B |
| array.ToList().Add(member)            | 100     |        155.2842 ns |       2.9093 ns |       2.7214 ns |        154.7431 ns |     2480 B |
| array.AddRange(membersArray)          | 100     |         90.4790 ns |       0.9390 ns |       0.8784 ns |         90.4858 ns |      984 B |
| array.ToList().AddRange(membersArray) | 100     |        179.3185 ns |       1.2501 ns |       0.9760 ns |        179.2334 ns |     2480 B |
| array.Remove(member)                  | 100     |         95.1714 ns |       0.7806 ns |       0.7301 ns |         95.1428 ns |      816 B |
| array.ToList().Remove(member)         | 100     |        113.5840 ns |       2.2275 ns |       3.1226 ns |        112.2163 ns |      856 B |
| array.RemoveAll(condition)            | 100     |        296.2594 ns |       1.0538 ns |       0.9342 ns |        295.9815 ns |      104 B |
| array.ToList().RemoveAll(condition)   | 100     |        788.2978 ns |      11.6702 ns |       9.1113 ns |        783.7741 ns |      920 B |
| list.ForAll()                         | 100000  |    292,220.4804 ns |   5,761.5403 ns |  15,278.7875 ns |    285,953.0762 ns |          - |
| list.ForEach()                        | 100000  |    276,478.7737 ns |   2,123.5251 ns |   1,882.4497 ns |    276,278.2715 ns |          - |
| foreach in list                       | 100000  |    210,212.1754 ns |   4,031.4752 ns |   4,140.0282 ns |    208,087.3779 ns |          - |
| for in list                           | 100000  |    214,975.3812 ns |   3,042.3258 ns |   2,540.4798 ns |    214,497.4121 ns |          - |
| list.ToList()                         | 100000  |    252,593.3865 ns |   4,841.7658 ns |   5,180.6351 ns |    250,724.9023 ns |   800082 B |
| list.AsList()                         | 100000  |          0.2416 ns |       0.0240 ns |       0.0225 ns |          0.2539 ns |          - |
| array.ToList()                        | 100000  |    256,933.4488 ns |   5,082.0885 ns |   7,912.1995 ns |    256,979.5654 ns |   800078 B |
| array.AsList()                        | 100000  |    256,735.4662 ns |   5,097.6930 ns |   6,446.9495 ns |    257,324.0723 ns |   800078 B |
| list.ToArray()                        | 100000  |    241,724.3193 ns |   4,741.1296 ns |   9,577.3157 ns |    243,516.0889 ns |   800016 B |
| list.AsArray()                        | 100000  |    239,297.2168 ns |   4,782.4278 ns |  12,765.2679 ns |    239,194.1895 ns |   800015 B |
| array.ToArray()                       | 100000  |    242,073.1110 ns |   4,839.2595 ns |  11,779.4264 ns |    241,527.4170 ns |   800020 B |
| array.AsArray()                       | 100000  |          0.1980 ns |       0.0017 ns |       0.0014 ns |          0.1974 ns |          - |
| array.Add(member)                     | 100000  |    252,521.3756 ns |   5,040.3855 ns |   8,281.4979 ns |    253,188.5742 ns |   800027 B |
| array.ToList().Add(member)            | 100000  |    533,333.6227 ns |  10,594.0729 ns |  14,851.4107 ns |    535,023.3398 ns |  2400145 B |
| array.AddRange(membersArray)          | 100000  |    243,777.1592 ns |   4,868.3471 ns |  13,651.3672 ns |    246,461.5234 ns |   800178 B |
| array.ToList().AddRange(membersArray) | 100000  |    529,034.6973 ns |  10,439.2022 ns |  15,624.9064 ns |    528,342.7246 ns |  2400140 B |
| array.Remove(member)                  | 100000  |    239,253.7050 ns |   4,757.1252 ns |  12,615.2211 ns |    241,452.5146 ns |   800007 B |
| array.ToList().Remove(member)         | 100000  |    276,216.6936 ns |   5,389.4241 ns |   7,377.1046 ns |    273,623.7793 ns |   800078 B |
| array.RemoveAll(condition)            | 100000  |    722,547.2389 ns |  14,375.7507 ns |  30,007.5451 ns |    717,823.3887 ns |   800381 B |
| array.ToList().RemoveAll(condition)   | 100000  |    893,255.9635 ns |   4,206.0163 ns |   3,934.3103 ns |    894,714.9414 ns |   800472 B |
| list.ForAll()                         | 1000000 |  5,465,727.5260 ns |  21,304.9198 ns |  19,928.6352 ns |  5,469,512.1094 ns |        8 B |
| list.ForEach()                        | 1000000 |  5,485,919.6429 ns |  24,291.6617 ns |  21,533.9254 ns |  5,483,908.5938 ns |        7 B |
| foreach in list                       | 1000000 |  5,376,657.1875 ns |  23,939.9112 ns |  22,393.4078 ns |  5,379,749.2188 ns |        7 B |
| for in list                           | 1000000 |  5,354,379.4792 ns |  18,013.1910 ns |  16,849.5500 ns |  5,352,898.4375 ns |        7 B |
| list.ToList()                         | 1000000 |  4,835,544.0625 ns |  96,588.2353 ns |  90,348.6952 ns |  4,862,678.9062 ns |  8000110 B |
| list.AsList()                         | 1000000 |          0.2014 ns |       0.0035 ns |       0.0031 ns |          0.2017 ns |          - |
| array.ToList()                        | 1000000 |  4,806,206.1979 ns |  84,893.6821 ns |  79,409.6029 ns |  4,828,608.5938 ns |  8000110 B |
| array.AsList()                        | 1000000 |  4,804,540.5208 ns |  73,318.8556 ns |  68,582.5030 ns |  4,831,218.7500 ns |  8000110 B |
| list.ToArray()                        | 1000000 |  4,799,392.3698 ns |  79,456.9603 ns |  74,324.0900 ns |  4,798,543.3594 ns |  8000078 B |
| list.AsArray()                        | 1000000 |  4,811,368.9062 ns |  78,422.3492 ns |  73,356.3141 ns |  4,844,948.4375 ns |  8000078 B |
| array.ToArray()                       | 1000000 |  4,813,089.2708 ns |  86,918.1579 ns |  81,303.2988 ns |  4,792,583.5938 ns |  8000078 B |
| array.AsArray()                       | 1000000 |          0.2083 ns |       0.0027 ns |       0.0021 ns |          0.2091 ns |          - |
| array.Add(member)                     | 1000000 |  4,807,320.6510 ns |  76,878.1966 ns |  71,911.9128 ns |  4,801,109.7656 ns |  8000086 B |
| array.ToList().Add(member)            | 1000000 |  6,090,543.4814 ns | 121,454.9199 ns | 281,490.2453 ns |  6,055,098.8281 ns | 24000482 B |
| array.AddRange(membersArray)          | 1000000 |  4,808,702.2917 ns |  76,213.8944 ns |  71,290.5241 ns |  4,826,896.8750 ns |  8000238 B |
| array.ToList().AddRange(membersArray) | 1000000 |  6,095,736.6655 ns | 120,577.4866 ns | 226,473.7473 ns |  6,063,239.8438 ns | 24000550 B |
| array.Remove(member)                  | 1000000 |  4,817,705.2865 ns |  94,062.6872 ns |  87,986.2960 ns |  4,844,812.1094 ns |  8000070 B |
| array.ToList().Remove(member)         | 1000000 |  5,475,281.3021 ns |  84,528.3341 ns |  79,067.8562 ns |  5,476,521.8750 ns |  8000110 B |
| array.RemoveAll(condition)            | 1000000 | 13,296,286.8304 ns |  80,477.7317 ns |  71,341.4129 ns | 13,304,075.7812 ns |  8000135 B |
| array.ToList().RemoveAll(condition)   | 1000000 | 13,825,874.2188 ns |  59,145.1058 ns |  52,430.5958 ns | 13,820,512.5000 ns |  8000175 B |

Conclusions

- Array `Add` extensions become less useful after 3 calls.
  - Small sized arrays run 3x faster and with 3x less memory.
  - Medium sized arrays run 2x+ faster and with 3x less memory.
  - Large sized arrays run at about the same speed but 3x less memory.
  - After 3 calls, the cost of creating a new list is surpassed for using lists.
- Array `Remove` extension breaks even typically. Suggest sticking to 3x or less rule.
- Array `RemoveAll` shows significant gains in small arrays but balances out in large arrays.
  - Needs more investigation.
- `ForAll` on a small list is comparable to `ForEach`.
- `ForAll` on a large list scales to compare to the faster `foreach` and `for`.
  - So you may want to use `ForAll` over `ForEach` on lists due to a similar floor and much higher ceiling.
- `AsArray` and `AsList` are extremely valuable when the collection was already an array or list respectively.
  - Both in performance speed and memory allocation.

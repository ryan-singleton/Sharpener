## Benchmarks

These are the most recent benchmarks to help in optimizing this library.

### String Comparison

| Method                                                           | Count   |      Mean |     Error |    StdDev | Allocated |
| ---------------------------------------------------------------- | ------- | --------: | --------: | --------: | --------: |
| string.Equals(string)                                            | 100     |  2.427 ns | 0.0240 ns | 0.0213 ns |         - |
| string.Equals(string, StringComparison.Ordinal)                  | 100     |  4.205 ns | 0.0562 ns | 0.0526 ns |         - |
| string.Case().Equals(string)                                     | 100     |  4.964 ns | 0.0795 ns | 0.0744 ns |         - |
| string.Equals(string, StringComparison.OrdinalIgnoreCase)        | 100     |  5.260 ns | 0.0194 ns | 0.0182 ns |         - |
| string.NoCase().Equals(string)                                   | 100     |  5.819 ns | 0.0523 ns | 0.0489 ns |         - |
| string.Equals(string, StringComparison.CurrentCultureIgnoreCase) | 100     | 73.247 ns | 0.7684 ns | 0.6812 ns |         - |
| string.NoCase().Current().Equals(string)                         | 100     | 81.383 ns | 0.7167 ns | 0.6704 ns |      32 B |
| string.Current().NoCase().Equals(string)                         | 100     | 83.367 ns | 0.5287 ns | 0.4945 ns |      32 B |
| string.Equals(string)                                            | 100000  |  2.378 ns | 0.0267 ns | 0.0250 ns |         - |
| string.Equals(string, StringComparison.Ordinal)                  | 100000  |  4.056 ns | 0.0582 ns | 0.0454 ns |         - |
| string.Case().Equals(string)                                     | 100000  |  5.180 ns | 0.0407 ns | 0.0361 ns |         - |
| string.Equals(string, StringComparison.OrdinalIgnoreCase)        | 100000  |  6.068 ns | 0.0476 ns | 0.0422 ns |         - |
| string.NoCase().Equals(string)                                   | 100000  |  6.196 ns | 0.0300 ns | 0.0266 ns |         - |
| string.Equals(string, StringComparison.CurrentCultureIgnoreCase) | 100000  | 71.986 ns | 0.7628 ns | 0.7136 ns |         - |
| string.NoCase().Current().Equals(string)                         | 100000  | 80.930 ns | 0.8876 ns | 0.7869 ns |      32 B |
| string.Current().NoCase().Equals(string)                         | 100000  | 85.931 ns | 1.0411 ns | 0.9739 ns |      32 B |
| string.Equals(string)                                            | 1000000 |  2.412 ns | 0.0431 ns | 0.0403 ns |         - |
| string.Equals(string, StringComparison.Ordinal)                  | 1000000 |  4.241 ns | 0.0697 ns | 0.0582 ns |         - |
| string.Case().Equals(string)                                     | 1000000 |  5.158 ns | 0.0475 ns | 0.0444 ns |         - |
| string.Equals(string, StringComparison.OrdinalIgnoreCase)        | 1000000 |  6.179 ns | 0.0975 ns | 0.0912 ns |         - |
| string.NoCase().Equals(string)                                   | 1000000 |  6.611 ns | 0.0448 ns | 0.0419 ns |         - |
| string.Equals(string, StringComparison.CurrentCultureIgnoreCase) | 1000000 | 72.070 ns | 0.4106 ns | 0.3841 ns |         - |
| string.NoCase().Current().Equals(string)                         | 1000000 | 82.004 ns | 0.7877 ns | 0.6983 ns |      32 B |
| string.Current().NoCase().Equals(string)                         | 1000000 | 84.123 ns | 0.9370 ns | 0.8306 ns |      32 B |

Conclusions

-   The instantiated structs only occur with culture builders and don't matter much. It's 32 B no matter the amount in the list.
-   Two to ten nanosecond increases, no matter the size of the collection, on evaluation.
-   It would be good if they can eventually offer performance and memory boosts, but for now, they have insignificantly small impacts.

### Collection Extensions

| Method          | Count   |              Mean |           Error |          StdDev | Allocated |
| --------------- | ------- | ----------------: | --------------: | --------------: | --------: |
| list.ForAll()   | 100     |       276.7443 ns |       2.7105 ns |       2.5354 ns |         - |
| list.ForEach()  | 100     |       301.7211 ns |       0.7826 ns |       0.6937 ns |         - |
| foreach in list | 100     |       177.2468 ns |       1.4633 ns |       1.2971 ns |         - |
| for in list     | 100     |       163.0307 ns |       0.4434 ns |       0.3931 ns |         - |
| list.ToList()   | 100     |        64.1417 ns |       1.3106 ns |       1.5093 ns |     856 B |
| list.AsList()   | 100     |         0.4250 ns |       0.0057 ns |       0.0053 ns |         - |
| array.ToList()  | 100     |        75.2693 ns |       1.4624 ns |       1.3680 ns |     856 B |
| array.AsList()  | 100     |        73.0393 ns |       1.4256 ns |       1.4001 ns |     856 B |
| list.ToArray()  | 100     |        46.4578 ns |       0.9111 ns |       0.8948 ns |     824 B |
| list.AsArray()  | 100     |        55.0068 ns |       0.9504 ns |       0.7936 ns |     824 B |
| array.ToArray() | 100     |        64.2055 ns |       0.6065 ns |       0.5065 ns |     824 B |
| array.AsArray() | 100     |         0.1886 ns |       0.0090 ns |       0.0084 ns |         - |
| list.ForAll()   | 100000  |   287,909.0983 ns |   5,485.8041 ns |   5,131.4246 ns |         - |
| list.ForEach()  | 100000  |   291,692.2175 ns |   3,039.5937 ns |   2,538.1984 ns |         - |
| foreach in list | 100000  |   216,395.7921 ns |   2,495.4151 ns |   2,212.1205 ns |         - |
| for in list     | 100000  |   218,534.6530 ns |   4,172.0463 ns |   4,464.0428 ns |         - |
| list.ToList()   | 100000  |   252,718.2861 ns |   4,959.4852 ns |   5,093.0261 ns |  800077 B |
| list.AsList()   | 100000  |         0.4119 ns |       0.0045 ns |       0.0038 ns |         - |
| array.ToList()  | 100000  |   253,674.9105 ns |   4,956.9522 ns |   4,636.7361 ns |  800077 B |
| array.AsList()  | 100000  |   254,393.9299 ns |   4,942.8014 ns |   6,929.1173 ns |  800077 B |
| list.ToArray()  | 100000  |   243,572.6465 ns |   4,842.6628 ns |   9,782.4176 ns |  800017 B |
| list.AsArray()  | 100000  |   245,680.5845 ns |   4,898.3488 ns |  10,332.2801 ns |  800018 B |
| array.ToArray() | 100000  |   247,671.3898 ns |   4,912.9319 ns |   8,604.6038 ns |  800019 B |
| array.AsArray() | 100000  |         0.1769 ns |       0.0103 ns |       0.0080 ns |         - |
| list.ForAll()   | 1000000 | 5,708,381.5394 ns | 113,255.0414 ns | 158,767.7506 ns |       6 B |
| list.ForEach()  | 1000000 | 5,536,317.9688 ns |  70,495.8237 ns |  62,492.7115 ns |       7 B |
| foreach in list | 1000000 | 5,533,211.2109 ns | 109,790.3287 ns | 126,434.7058 ns |       6 B |
| for in list     | 1000000 | 5,750,210.3824 ns | 113,242.7718 ns | 195,338.2174 ns |       6 B |
| list.ToList()   | 1000000 | 4,981,261.0547 ns |  96,818.5927 ns | 111,496.4354 ns | 8000117 B |
| list.AsList()   | 1000000 |         0.2208 ns |       0.0066 ns |       0.0059 ns |         - |
| array.ToList()  | 1000000 | 4,923,324.4062 ns |  95,517.2278 ns | 127,512.7556 ns | 8000110 B |
| array.AsList()  | 1000000 | 4,958,136.1003 ns |  96,308.5325 ns | 125,228.2959 ns | 8000110 B |
| list.ToArray()  | 1000000 | 4,882,903.1771 ns |  77,519.0912 ns |  72,511.4060 ns | 8000078 B |
| list.AsArray()  | 1000000 | 4,954,761.0491 ns |  77,001.7723 ns |  68,260.0655 ns | 8000078 B |
| array.ToArray() | 1000000 | 4,886,470.4799 ns |  90,592.9937 ns |  80,308.3292 ns | 8000078 B |
| array.AsArray() | 1000000 |         0.2010 ns |       0.0113 ns |       0.0105 ns |         - |

Conclusions

-   `ForAll` on a small list is comparable to `ForEach`.
-   `ForAll` on a large list scales to compare to the faster `foreach` and `for`.
    -   So you may want to use `ForAll` over `ForEach` on lists due to a similar floor and much higher ceiling.
-   `AsArray` and `AsList` are extremely valuable when the collection was already an array or list respectively.
    -   Both in performance speed and memory allocation.

## Benchmarks

These are the most recent benchmarks to help in optimizing this library. Note that this means that some negative results will relate to an issue that's being worked on.

### String Comparison

| Method                    | Count   |      Mean |     Error |    StdDev | Allocated |
| ------------------------- | ------- | --------: | --------: | --------: | --------: |
| Equals_Original           | 100     |  2.524 ns | 0.0672 ns | 0.0629 ns |         - |
| Case_Equivalent           | 100     |  3.921 ns | 0.0320 ns | 0.0299 ns |         - |
| Case                      | 100     |  5.095 ns | 0.0274 ns | 0.0243 ns |         - |
| NoCase_Equivalent         | 100     |  5.912 ns | 0.0307 ns | 0.0272 ns |         - |
| NoCase                    | 100     |  6.425 ns | 0.0625 ns | 0.0554 ns |         - |
| NoCase_Current_Equivalent | 100     | 69.622 ns | 0.3346 ns | 0.3130 ns |         - |
| NoCase_Current            | 100     | 81.523 ns | 0.5534 ns | 0.4905 ns |      32 B |
| Current_NoCase            | 100     | 80.052 ns | 0.5772 ns | 0.5399 ns |      32 B |
| Equals_Original           | 100000  |  2.304 ns | 0.0165 ns | 0.0146 ns |         - |
| Case_Equivalent           | 100000  |  4.084 ns | 0.0411 ns | 0.0384 ns |         - |
| Case                      | 100000  |  5.109 ns | 0.0501 ns | 0.0468 ns |         - |
| NoCase_Equivalent         | 100000  |  5.942 ns | 0.0418 ns | 0.0391 ns |         - |
| NoCase                    | 100000  |  6.433 ns | 0.0325 ns | 0.0288 ns |         - |
| NoCase_Current_Equivalent | 100000  | 72.936 ns | 0.5623 ns | 0.5260 ns |         - |
| NoCase_Current            | 100000  | 79.214 ns | 0.6234 ns | 0.5831 ns |      32 B |
| Current_NoCase            | 100000  | 84.198 ns | 0.6682 ns | 0.5924 ns |      32 B |
| Equals_Original           | 1000000 |  2.304 ns | 0.0194 ns | 0.0181 ns |         - |
| Case_Equivalent           | 1000000 |  3.913 ns | 0.0373 ns | 0.0349 ns |         - |
| Case                      | 1000000 |  5.089 ns | 0.0347 ns | 0.0307 ns |         - |
| NoCase_Equivalent         | 1000000 |  5.788 ns | 0.0528 ns | 0.0494 ns |         - |
| NoCase                    | 1000000 |  6.418 ns | 0.0637 ns | 0.0596 ns |         - |
| NoCase_Current_Equivalent | 1000000 | 69.650 ns | 0.6207 ns | 0.5806 ns |         - |
| NoCase_Current            | 1000000 | 82.122 ns | 0.4619 ns | 0.4095 ns |      32 B |
| Current_NoCase            | 1000000 | 79.810 ns | 1.2678 ns | 1.1859 ns |      32 B |

### Collection Extensions

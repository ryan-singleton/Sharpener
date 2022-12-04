## Benchmarks

These are the most recent benchmarks to help in optimizing this library.

### String Comparison

| Method                    | Count   |      Mean |     Error |    StdDev | Allocated |
| ------------------------- | ------- | --------: | --------: | --------: | --------: |
| Equals_Original           | 100     |  2.929 ns | 0.0476 ns | 0.0422 ns |         - |
| Case_Equivalent           | 100     |  4.553 ns | 0.0972 ns | 0.1229 ns |         - |
| Case                      | 100     |  5.548 ns | 0.0517 ns | 0.0458 ns |         - |
| NoCase_Equivalent         | 100     |  5.912 ns | 0.1181 ns | 0.1047 ns |         - |
| NoCase                    | 100     |  6.617 ns | 0.1206 ns | 0.1007 ns |         - |
| NoCase_Current_Equivalent | 100     | 86.109 ns | 1.1258 ns | 0.8789 ns |         - |
| NoCase_Current            | 100     | 91.081 ns | 1.2140 ns | 1.0138 ns |      32 B |
| Current_NoCase            | 100     | 91.917 ns | 1.7946 ns | 1.9202 ns |      32 B |
| Equals_Original           | 100000  |  2.654 ns | 0.0668 ns | 0.0625 ns |         - |
| Case_Equivalent           | 100000  |  4.459 ns | 0.0827 ns | 0.0773 ns |         - |
| Case                      | 100000  |  5.789 ns | 0.1425 ns | 0.1400 ns |         - |
| NoCase_Equivalent         | 100000  |  7.119 ns | 0.1683 ns | 0.2247 ns |         - |
| NoCase                    | 100000  |  7.587 ns | 0.1781 ns | 0.1979 ns |         - |
| NoCase_Current_Equivalent | 100000  | 86.340 ns | 1.5998 ns | 1.3359 ns |         - |
| NoCase_Current            | 100000  | 92.272 ns | 1.6245 ns | 1.5196 ns |      32 B |
| Current_NoCase            | 100000  | 92.298 ns | 1.1471 ns | 0.9579 ns |      32 B |
| Equals_Original           | 1000000 |  2.826 ns | 0.0806 ns | 0.0827 ns |         - |
| Case_Equivalent           | 1000000 |  4.912 ns | 0.1010 ns | 0.0945 ns |         - |
| Case                      | 1000000 |  5.454 ns | 0.1288 ns | 0.1142 ns |         - |
| NoCase_Equivalent         | 1000000 |  6.687 ns | 0.1293 ns | 0.1209 ns |         - |
| NoCase                    | 1000000 |  7.503 ns | 0.1733 ns | 0.2192 ns |         - |
| NoCase_Current_Equivalent | 1000000 | 85.047 ns | 1.7058 ns | 1.5956 ns |         - |
| NoCase_Current            | 1000000 | 99.367 ns | 1.6440 ns | 1.4573 ns |      32 B |
| Current_NoCase            | 1000000 | 94.614 ns | 1.5016 ns | 1.4748 ns |      32 B |

Conclusions

-   The instantiated structs don't matter much. It's 32 B no matter the amount in the list. Otherwise, no memory notes.
-   Two to ten nanosecond increases, no matter the size of the collection, on evaluation.
-   So the string comparisons do not (yet) offer performance boosts, but the performance and memory impacts are insignificant.

### Collection Extensions

| Method               | Count   |               Mean |           Error |          StdDev |             Median | Allocated |
| -------------------- | ------- | -----------------: | --------------: | --------------: | -----------------: | --------: |
| ForAll               | 100     |        278.5830 ns |       5.2338 ns |       4.8957 ns |        277.7639 ns |         - |
| ForEach              | 100     |        282.6910 ns |       3.4965 ns |       3.2707 ns |        282.9651 ns |         - |
| ForEachManual        | 100     |        178.8327 ns |       1.5853 ns |       1.4053 ns |        179.0663 ns |         - |
| ForManual            | 100     |        157.7902 ns |       1.9481 ns |       1.8223 ns |        158.0861 ns |         - |
| ToList_WhenWasList   | 100     |         64.0326 ns |       1.3091 ns |       2.1508 ns |         63.6619 ns |     856 B |
| AsList_WhenWasList   | 100     |          0.2299 ns |       0.0092 ns |       0.0086 ns |          0.2273 ns |         - |
| ToList               | 100     |         72.5005 ns |       0.9645 ns |       1.3202 ns |         72.0109 ns |     856 B |
| AsList               | 100     |         74.9609 ns |       1.3535 ns |       1.2661 ns |         74.7619 ns |     856 B |
| ToArray              | 100     |         47.2700 ns |       0.9682 ns |       1.7209 ns |         46.9711 ns |     824 B |
| AsArray              | 100     |         59.9532 ns |       1.2233 ns |       3.3071 ns |         58.7721 ns |     824 B |
| ToArray_WhenWasArray | 100     |         67.1285 ns |       1.1427 ns |       2.2017 ns |         66.6446 ns |     824 B |
| AsArray_WhenWasArray | 100     |          0.3919 ns |       0.0351 ns |       0.0536 ns |          0.3706 ns |         - |
| ForAll               | 100000  |    448,445.9653 ns |  27,307.8587 ns |  80,517.8376 ns |    473,748.0469 ns |         - |
| ForEach              | 100000  |    547,955.5345 ns |  10,905.9598 ns |  28,730.6604 ns |    545,512.3535 ns |       1 B |
| ForEachManual        | 100000  |    455,719.6996 ns |  11,869.8500 ns |  34,247.2313 ns |    454,778.8574 ns |         - |
| ForManual            | 100000  |    447,952.6781 ns |  25,052.7864 ns |  73,475.4857 ns |    430,797.6562 ns |         - |
| ToList_WhenWasList   | 100000  |    317,654.6980 ns |   4,614.8336 ns |   4,090.9298 ns |    318,296.6797 ns |  800077 B |
| AsList_WhenWasList   | 100000  |          0.4365 ns |       0.0118 ns |       0.0092 ns |          0.4400 ns |         - |
| ToList               | 100000  |    393,143.3192 ns |   5,928.3358 ns |   6,087.9644 ns |    393,260.8887 ns |  800076 B |
| AsList               | 100000  |    376,035.4838 ns |   7,298.1899 ns |   9,489.7084 ns |    375,031.9336 ns |  800077 B |
| ToArray              | 100000  |    305,353.0170 ns |   9,604.9748 ns |  28,169.7285 ns |    314,936.4258 ns |  800019 B |
| AsArray              | 100000  |    322,609.4829 ns |   8,523.9189 ns |  25,132.9673 ns |    328,348.9746 ns |  800019 B |
| ToArray_WhenWasArray | 100000  |    331,187.3271 ns |   7,339.7698 ns |  21,641.4769 ns |    338,814.5264 ns |  800019 B |
| AsArray_WhenWasArray | 100000  |          0.4705 ns |       0.0299 ns |       0.0280 ns |          0.4654 ns |         - |
| ForAll               | 1000000 | 10,187,016.4982 ns | 197,054.0424 ns | 202,359.9935 ns | 10,213,642.9688 ns |      13 B |
| ForEach              | 1000000 | 10,548,920.7933 ns | 203,769.5424 ns | 278,922.0496 ns | 10,550,437.5000 ns |      13 B |
| ForEachManual        | 1000000 | 10,394,851.1285 ns | 197,569.9269 ns | 211,397.6083 ns | 10,398,446.0938 ns |      13 B |
| ForManual            | 1000000 | 10,127,879.7423 ns | 200,666.3684 ns | 436,232.1704 ns | 10,208,195.3125 ns |      13 B |
| ToList_WhenWasList   | 1000000 |  7,005,835.8758 ns | 137,351.4805 ns | 349,603.3724 ns |  7,082,775.7812 ns | 8000111 B |
| AsList_WhenWasList   | 1000000 |          0.2096 ns |       0.0209 ns |       0.0185 ns |          0.2060 ns |         - |
| ToList               | 1000000 |  7,681,151.5425 ns | 149,540.8252 ns | 261,908.6923 ns |  7,771,231.2500 ns | 8000110 B |
| AsList               | 1000000 |  7,582,168.0990 ns | 107,558.8266 ns | 100,610.5931 ns |  7,586,458.9844 ns | 8000110 B |
| ToArray              | 1000000 |  5,193,217.0647 ns | 103,519.0820 ns | 170,084.8204 ns |  5,193,281.6406 ns | 8000078 B |
| AsArray              | 1000000 |  7,024,308.8802 ns | 119,899.5192 ns | 112,154.0847 ns |  7,018,780.0781 ns | 8000078 B |
| ToArray_WhenWasArray | 1000000 |  7,304,243.0469 ns | 143,388.8682 ns | 254,873.5674 ns |  7,303,213.2812 ns | 8000075 B |
| AsArray_WhenWasArray | 1000000 |          0.4446 ns |       0.0256 ns |       0.0240 ns |          0.4532 ns |         - |

Conclusions

-   `ForAll` on a small list is comparable to `ForEach`.
-   `ForAll` on a large list is comparable to manual `foreach` and `for` at scale than it is to `ForEach`.
    -   So you may want to use `ForAll` even on lists due to similar start metrics and better scaling.
-   `AsArray` and `AsList` are extremely valuable when the collection was already an array or list respectively.
    -   Both in performance speed and memory allocation.

```

BenchmarkDotNet v0.15.2, macOS Sequoia 15.5 (24F74) [Darwin 24.5.0]
Apple M1, 1 CPU, 8 logical and 8 physical cores
.NET SDK 9.0.301
  [Host]     : .NET 9.0.6 (9.0.625.26613), Arm64 RyuJIT AdvSIMD
  DefaultJob : .NET 9.0.6 (9.0.625.26613), Arm64 RyuJIT AdvSIMD


```
| Method   | Mean     | Error     | StdDev    |
|--------- |---------:|----------:|----------:|
| Sorted   | 9.165 μs | 0.1451 μs | 0.1357 μs |
| Unsorted | 6.480 μs | 0.1286 μs | 0.2656 μs |

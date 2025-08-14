
BenchmarkDotNet v0.15.2, macOS Sequoia 15.5 (24F74) [Darwin 24.5.0]
Apple M1, 1 CPU, 8 logical and 8 physical cores
.NET SDK 9.0.301
  [Host]   : .NET 9.0.6 (9.0.625.26613), Arm64 RyuJIT AdvSIMD
  .NET 9.0 : .NET 9.0.6 (9.0.625.26613), Arm64 RyuJIT AdvSIMD

Job=.NET 9.0  Runtime=.NET 9.0  

 Method  | Mean     | Error    | StdDev   |
-------- |---------:|---------:|---------:|
 IsMatch | 23.29 ns | 0.218 ns | 0.193 ns |

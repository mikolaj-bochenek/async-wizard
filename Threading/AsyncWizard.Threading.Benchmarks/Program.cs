using BenchmarkDotNet.Running;

namespace AsyncWizard.Threading.Benchmarks;

public class Program
{
    static void Main(string[] args)
    {
        BenchmarkRunner.Run<ThreadingHelpersBenchmarks>();
    }
}

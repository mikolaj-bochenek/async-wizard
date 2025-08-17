using System.Security.Cryptography;
using AsyncWizard.Threading.Core;
using BenchmarkDotNet.Attributes;

namespace AsyncWizard.Threading.Benchmarks;

public class ThreadingHelpersBenchmarks
{
    private readonly SHA256 sha256 = SHA256.Create();
    private byte[] data = [];

    [GlobalSetup]
    public void Setup()
    {
        data = new byte[1000];
        new Random(42).NextBytes(data);
    }

    [Benchmark]
    public void ExecuteSynchronously() => sha256.ComputeHash(data);

    [Benchmark]
    public void ExecuteOnThread()
    {
        ThreadingHelpers.ExecuteOnThread(() => sha256.ComputeHash(data), 1);
    }

    [Benchmark]
    public void ExecuteOnThreadPool()
    {
        ThreadingHelpers.ExecuteOnThreadPool(() => sha256.ComputeHash(data), 1);
    }
}

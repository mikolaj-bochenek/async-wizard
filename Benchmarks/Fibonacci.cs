namespace AsyncWizard.Benchmarks;

// [DisassemblyDiagnoser(exportCombinedDisassemblyReport: true)]
[MemoryDiagnoser]
public class Fibonacci
{
    private readonly Dictionary<ulong, ulong> _cache = [];

    [Benchmark(Baseline = true)]
    [ArgumentsSource(nameof(Data))]
    public ulong Recursive(ulong n)
    {
        if (n == 1 || n == 2) return 1;
        return Recursive(n - 1) + Recursive(n - 2);
    }

    [Benchmark]
    [ArgumentsSource(nameof(Data))]
    public ulong RecursiveWithMemoization(ulong n)
    {
        if (_cache.TryGetValue(n, out var result)) return result;

        if (n == 1 || n == 2)
        {
            _cache[n] = 1;
            return 1;
        }

        result = RecursiveWithMemoization(n - 1) + RecursiveWithMemoization(n - 2);
        _cache[n] = result;
        return result;
    }

    [Benchmark]
    [ArgumentsSource(nameof(Data))]
    public ulong Iterative(ulong n)
    {
        if (n == 1 || n == 2) return 1;

        ulong prev1 = 1, prev2 = 1, current = 0;

        for (ulong i = 3; i <= n; i++)
        {
            current = prev1 + prev2;
            prev1 = prev2;
            prev2 = current;
        }

        return current;
    }

    public IEnumerable<ulong> Data()
    {
        yield return 15;
        yield return 35;
    }
}
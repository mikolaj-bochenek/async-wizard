namespace AsyncWizard.Benchmarks;

[HtmlExporter]
[SimpleJob(RuntimeMoniker.Net90)]
public class MD5vsSHA256
{
    private readonly SHA256 _sha256 = SHA256.Create();
    private readonly MD5 _md5 = MD5.Create();
    private byte[] data = [];

    [Params(1000, 10000)]
    public int N;

    [GlobalSetup]
    public void Setup()
    {
        data = new byte[N];
        new Random(42).NextBytes(data);
    }

    [Benchmark]
    public byte[] Sha256() => _sha256.ComputeHash(data);

    [Benchmark]
    public byte[] Md5() => _md5.ComputeHash(data);
}

public class TimersSample
{
    [Benchmark]
    public static void Fast() => Thread.Sleep(50);

    [Benchmark(Baseline = true)]
    public static void Normal() => Thread.Sleep(100);

    [Benchmark]
    public static void Slow() => Thread.Sleep(150);
}

[SimpleJob(RuntimeMoniker.Net90)]
[MarkdownExporter]
public class RegexPerformance
{
    private readonly Regex _regex = new("[a-zA-Z0-9]*", RegexOptions.Compiled);

    [Benchmark]
    public bool IsMatch() => _regex.IsMatch("abcdefghijklmnopqrstuvwxyz123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ");
}

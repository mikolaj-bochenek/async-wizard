using AsyncWizard.Benchmarks;

namespace AsyncWizard;

public class Program
{
    public static void Main()
    {
        // BenchmarkRunner.Run<MD5vsSHA256>();
        // BenchmarkRunner.Run<TimersSample>();
        // BenchmarkRunner.Run<RegexPerformance>();
        // BenchmarkRunner.Run<StringAllocationSample>();
        // BenchmarkRunner.Run<MemorySamples>();
        // BenchmarkRunner.Run<ThreadingSamples>();
        BenchmarkRunner.Run<NuGetSamples>();

        var A = "abc";
        var B = "abc";

        Console.WriteLine(ReferenceEquals(A, B.ToUpper().ToLower())); // False: A and B are different references
        Console.WriteLine(ReferenceEquals(A, B)); // True: A and B are the same references because of interning
    }
}

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

[MemoryDiagnoser]
public class StringAllocationSample
{
    private const int N = 1000;

    [Benchmark]
    public string Concat()
    {
        string result = "";
        for (int i = 0; i < N; i++)
        {
            result += "lorem";
        }
        return result;
    }

    [Benchmark]
    public string StringBuilder()
    {
        var sb = new StringBuilder();
        for (int i = 0; i < N; i++)
        {
            sb.Append("lorem");
        }
        return sb.ToString();
    }
}

[MemoryDiagnoser]
// [NativeMemoryProfiler] // Windows only
[EventPipeProfiler(EventPipeProfile.GcVerbose)]
public class MemorySamples
{
    [Benchmark]
    public void DrawLine()
    {
        using var bmp = new SKBitmap(100, 100);
        using var canvas = new SKCanvas(bmp);
        using var paint = new SKPaint { Color = SKColors.Black, StrokeWidth = 2, IsAntialias = true };
        canvas.DrawLine(0, 0, 100, 100, paint);
    }

    [Benchmark]
    public void DrawLineWithoutDisposing()
    {
        var bmp = new SKBitmap(100, 100);
        var canvas = new SKCanvas(bmp);
        var paint = new SKPaint { Color = SKColors.Black, StrokeWidth = 2, IsAntialias = true };
        canvas.DrawLine(0, 0, 100, 100, paint);
    }
}
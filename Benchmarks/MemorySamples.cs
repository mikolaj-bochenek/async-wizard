namespace AsyncWizard.Benchmarks;

// BenchmarkDotNet has a MemoryDiagnoser that can be used to get information about amount
// of bytes allocated by our method. This functionality is especially helpful during
// performance improvements based on removing allocations.

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

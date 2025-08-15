namespace AsyncWizard.Benchmarks;

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
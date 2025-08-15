namespace AsyncWizard.Benchmarks;

[ThreadingDiagnoser]
public class ThreadingSamples
{
    [Benchmark]
    public void CountFromTen()
    {
        var count = 10;
        using var e = new CountdownEvent(count);
        var locked = new object();

        for (int i = 0; i < count; i++)
        {
            ThreadPool.QueueUserWorkItem(m =>
            {
                lock (locked)
                {
                    (m as CountdownEvent)?.Signal();
                }
            }, e);
        }

        e.Wait();
    }
}

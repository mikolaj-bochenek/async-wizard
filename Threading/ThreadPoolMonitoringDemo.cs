using System.ComponentModel;

namespace AsyncWizard;

[Description("Monitoring thread pool")]
public class ThreadPoolMonitoringDemo
{
    /// <summary>
    /// Play around with this value to observe queuing and hill-climbing of worker threads:
    ///  1000 - some queuing but very slow increase of the worker threads
    ///  60000 - massive queuing and big increase of the worker threads
    /// </summary>
    private const int SingleWorkItemDelayMS = 1_000;

    public static void Run()
    {
        Console.Clear();

        ThreadPool.GetMinThreads(out var minWorkerThreads, out var minCompletionPortThreads);
        ThreadPool.GetMaxThreads(out var maxWorkerThreads, out var maxCompletionPortThreads);

        Console.WriteLine($"Worker threads (min: {minWorkerThreads}, max: {maxWorkerThreads})");
        Console.WriteLine($"IOCP threads: (min: {minCompletionPortThreads}, max: {maxCompletionPortThreads})");

        var statsThread = new Thread(PrintThreadPoolStats)
        {
            IsBackground = true
        };
        statsThread.Start();

        var spawningThread = new Thread(SpawnWork)
        {
            IsBackground = true
        };
        spawningThread.Start();

        Console.ReadLine();
    }

    private static void SpawnWork()
    {
        while (true)
        {
            Thread.Sleep(100);
            ThreadPool.QueueUserWorkItem(DoWork);
        }
    }

    private static void DoWork(object? arg)
    {
        Console.CursorTop = 3;
        Console.Write(".");
        Thread.Sleep(SingleWorkItemDelayMS);
    }

    private static void PrintThreadPoolStats()
    {
        while (true)
        {
            Console.CursorLeft = 0;
            Console.CursorTop = 2;
            
            ThreadPool.GetAvailableThreads(out var workerThreads, out var completionPortThreads);

            Console.WriteLine($"Current: {ThreadPool.ThreadCount}, Queued: {ThreadPool.PendingWorkItemCount}, Done: {ThreadPool.CompletedWorkItemCount}, Worker: {workerThreads}, IOCP: {completionPortThreads}");

            Thread.Sleep(1000);
        }
    }
}
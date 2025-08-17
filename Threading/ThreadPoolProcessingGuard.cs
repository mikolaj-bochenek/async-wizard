namespace AsyncWizard.Threading;

public class ThreadPoolProcessingGuard
{
    private static AutoResetEvent guardRunningFlag = new AutoResetEvent(initialState: false);

    public static void Run(string[] args)
    {
        Console.ReadKey();
        Console.Clear();
        Console.CursorVisible = false;

        // To make queuing much easier
        ThreadPool.SetMinThreads(workerThreads: 8, completionPortThreads: 8);
        // You cannot set the maximum number of worker threads or I/O completion threads to a number smaller than the number of processors.
        ThreadPool.SetMaxThreads(workerThreads: Environment.ProcessorCount, completionPortThreads: 1000);

#if USE_GUARD
        ThreadPool.QueueUserWorkItem(ProcessingGuard);
        guardRunningFlag.WaitOne();
#endif

        Random rand = new Random();
        for (int i = 0; i < 20; ++i)
            ThreadPool.QueueUserWorkItem(DoWork, i);

        // App just ends, no waiting here
        // Console.ReadKey();
    }

    private static void DoWork(object? state)
    {
        var previousState = Thread.CurrentThread.IsBackground ? "B" : "F";
        const int Iterations = 10;
        int id = (int)state!;
        Debug(0, id + 1, str: $"{id:D2}{previousState}");
        for (int i = 0; i < Iterations; ++i)
        {
            Debug(i + 5, id + 1, ".");
            Thread.Sleep(1000);
        }
    }


    /// <summary>
    /// Thread routine checking if there are no pending work items in a ThreadPool
    /// queue for a given amount of time. As it is a foreground thread, it will
    /// block application close before all queued items are indeed started.
    /// Remark: it only guards enqueuing, if we want to make sure processing will
    /// be not interrupted, work items should be Foreground threads too.
    /// </summary>
    /// <param name="state"></param>
    private static void ProcessingGuard(object? state)
    {
        Thread.CurrentThread.IsBackground = false;
        guardRunningFlag.Set();

        const int EmptyQueueThresholdMilliseconds = 3_000;
        const int LoopDelayMilliseconds = 100;

        int emptyQueueCounter = 0;
        int emptyQueueThreshold = EmptyQueueThresholdMilliseconds / LoopDelayMilliseconds;

        while (true)
        {
            long queue = ThreadPool.PendingWorkItemCount;
            Debug(x: 0, y: 0, str: $"Guard Q:{queue:D2} C:{emptyQueueCounter:D2}");

            if (queue == 0)
            {
                emptyQueueCounter++;
                if (emptyQueueCounter >= emptyQueueThreshold)
                {
                    break;
                }
            }
            else
            {
                emptyQueueCounter = 0;
            }

            Thread.Sleep(LoopDelayMilliseconds);
        }

        Debug(0, 0, "G: done!          ");
    }

    private static object lockObj = new object();

    private static void Debug(int x, int y, string str)
    {
        lock (lockObj) // żeby kilka wątków nie pisało w tym samym czasie
        {
            try
            {
                Console.SetCursorPosition(x, y);
                Console.Write(str);
            }
            catch (ArgumentOutOfRangeException)
            {
                // Jeśli kursor poza ekranem (np. za małe okno konsoli) – ignorujemy
            }
        }
    }
}
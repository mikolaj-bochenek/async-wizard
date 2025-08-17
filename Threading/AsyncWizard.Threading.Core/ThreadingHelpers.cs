namespace AsyncWizard.Threading.Core;

public class ThreadingHelpers
{
    public static void ExecuteOnThread(Action action, int repeats, Action<Exception>? errorAction = null, CancellationToken token = default)
    {
        // * Create a thread and execute there `action` given number of `repeats` - waiting for the execution!
        //   HINT: you may use `Join` to wait until created Thread finishes
        // * In a loop, check whether `token` is not cancelled
        // * If an `action` throws and exception (or token has been cancelled) - `errorAction` should be invoked (if provided)

        ArgumentOutOfRangeException.ThrowIfNegative(repeats, nameof(repeats));

        if (token.IsCancellationRequested)
        {
            errorAction?.Invoke(new OperationCanceledException(token));
            return;
        }

        Exception? captured = null;

        var thread = new Thread(_ =>
        {
            try
            {
                for (int i = 0; i < repeats; i++)
                {
                    if (token.IsCancellationRequested)
                    {
                        captured = new OperationCanceledException(token);
                        break;
                    }

                    action();
                }
            }
            catch (Exception ex)
            {
                captured = ex;
            }
        })
        {
            IsBackground = true
        };

        thread.Start();
        thread.Join();

        if (captured is not null)
        {
            errorAction?.Invoke(captured);
        }
    }

    public static void ExecuteOnThreadPool(Action action, int repeats, Action<Exception>? errorAction = null, CancellationToken token = default)
    {
        // * Queue work item to a thread pool that executes `action` given number of `repeats` - waiting for the execution!
        //   HINT: you may use `AutoResetEvent` to wait until the queued work item finishes
        // * In a loop, check whether `token` is not cancelled
        // * If an `action` throws and exception (or token has been cancelled) - `errorAction` should be invoked (if provided)

        ArgumentOutOfRangeException.ThrowIfNegative(repeats, nameof(repeats));

        if (token.IsCancellationRequested)
        {
            errorAction?.Invoke(new OperationCanceledException(token));
            return;
        }

        using var done = new AutoResetEvent(false);
        Exception? captured = null;

        ThreadPool.QueueUserWorkItem(_ =>
        {
            try
            {
                for (int i = 0; i < repeats; i++)
                {
                    if (token.IsCancellationRequested)
                    {
                        captured = new OperationCanceledException(token);
                        break;
                    }

                    action();
                }
            }
            catch (Exception ex)
            {
                captured = ex;
            }
            finally
            {
                done.Set();
            }
        });

        done.WaitOne();

        if (captured is not null)
        {
            errorAction?.Invoke(captured);
        }
    }
}

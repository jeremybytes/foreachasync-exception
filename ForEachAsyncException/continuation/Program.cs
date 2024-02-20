using System.Text;

class Program
{
    private static int TotalProcessed = 0;
    private static int TotalExceptions = 0;

    static async Task Main(string[] args)
    {
        try
        {
            await Parallel.ForEachAsync(Enumerable.Range(1, 100),
                new ParallelOptions() { MaxDegreeOfParallelism = 10 },
                async (i, _) =>
                {
                    Console.WriteLine($"Processing item: {i}");
                    await Task.Delay(10); // simulate async task
                    MightThrowException(i);
                    Interlocked.Increment(ref TotalProcessed);
                })
                .ContinueWith(task =>
                {
                    ShowAggregateException(task.Exception!);
                },
                TaskContinuationOptions.OnlyOnFaulted);
        }
        catch (Exception)
        {
            // You never get to this code. Exceptions are handled
            // inside the ForEachAsync loop.

            // But just in case, rethrow the exception
            Console.WriteLine("You shouldn't get here");
            throw;
        }

        Console.WriteLine($"\nTotal Processed: {TotalProcessed}");
        Console.WriteLine($"Total Exceptions: {TotalExceptions}");
        Console.WriteLine("Done (AggregateException from Continuation)");
    }

    private static void MightThrowException(int item)
    {
        if (item % 3 == 0)
        {
            Interlocked.Increment(ref TotalExceptions);
            throw new Exception($"Bad thing happened inside loop ({item})");
        }
    }

    private static void ShowAggregateException(AggregateException ex)
    {
        StringBuilder builder = new();

        var innerExceptions = ex.Flatten().InnerExceptions;
        builder.AppendLine("======================");
        builder.AppendLine($"Aggregate Exception: Count {innerExceptions.Count}");

        foreach (var inner in innerExceptions)
            builder.AppendLine($"Continuation Exception: {inner!.Message}");
        builder.Append("======================");

        Console.WriteLine(builder.ToString());
    }
}

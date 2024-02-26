class Program
{
    private static int TotalProcessed = 0;
    private static int TotalExceptions = 0;

    static async Task Main(string[] args)
    {
        Console.Clear();
        try
        {
            await Parallel.ForEachAsync(
                Enumerable.Range(1, 100),
                new ParallelOptions() { MaxDegreeOfParallelism = 10 },
                async (i, _) =>
                {
                    Console.WriteLine($"Processing item: {i}");
                    await Task.Delay(10); // simulate async task
                    MightThrowException(i);
                    Interlocked.Increment(ref TotalProcessed);
                });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }

        Console.WriteLine($"\nTotal Processed: {TotalProcessed}");
        Console.WriteLine($"Total Exceptions: {TotalExceptions}");
        Console.WriteLine("Done (Original Code)");
    }

    private static void MightThrowException(int item)
    {
        if (item % 3 == 0)
        {
            Interlocked.Increment(ref TotalExceptions);
            throw new Exception($"Bad thing happened inside loop ({item})");
        }
    }
}

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

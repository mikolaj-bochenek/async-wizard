namespace AsyncWizard.Benchmarks;

// BenchmarkDotNet has a functionality that allows us to get Assembly and MSIL from our benchmark.
// It’s especially valuable during performance improvements, where ASM can explain optimization and the profits

[DisassemblyDiagnoser]
public class DisassemblySample
{
    private interface IPerson
    {
        int Age { get; }
    }

    private struct Person : IPerson
    {
        public Person(int age)
        {
            Age = age;
        }
        
        public int Age { get; }
    }

    private readonly Person _p = new Person(55);

    [Benchmark]
    public int WithInterface() => GetAge(_p);

    [Benchmark]
    public int WithConstrainedInterface() => GetAgeWithConstrained(_p);

    private static int GetAge(IPerson person) => person.Age;
    private static int GetAgeWithConstrained<T>(T person) where T : struct, IPerson => person.Age;
}
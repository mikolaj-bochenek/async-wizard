namespace AsyncWizard.Benchmarks;

class Student
{
    public Student(int age)
    {
        Age = age;
    }

    public int Age { get; }
    public bool IsAllowed() => Age >= 18;
}

class StudentService
{
    public static int CalculateAllowedStudents(Student[] students)
    {
        var count = 0;
        foreach (var student in students)
        {
            if (student.IsAllowed()) count++;
        }

        return count;
    }
}

[HardwareCounters(HardwareCounter.BranchMispredictions, HardwareCounter.BranchInstructions)]
public class Branching
{
    [GlobalSetup]
    public void Setup()
    {
        var count = 10_000;
        var students = new List<Student>(count);
        var random = new Random();
        for (var i = 0; i < count; i++)
        {
            var age = random.Next(15, 26);
            students.Add(new Student(age));
        }

        _sortedStudents = students.OrderBy(x => x.Age).ToArray();
        _unsortedStudents = students.ToArray();
    }

    private Student[] _sortedStudents;
    private Student[] _unsortedStudents;

    [Benchmark]
    public int Sorted() => StudentService.CalculateAllowedStudents(_sortedStudents);

    [Benchmark]
    public int Unsorted() => StudentService.CalculateAllowedStudents(_unsortedStudents);
}
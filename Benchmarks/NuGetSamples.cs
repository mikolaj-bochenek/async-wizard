
using BenchmarkDotNet.Configs;
using YamlDotNet.Serialization;

namespace AsyncWizard.Benchmarks;

[Config(typeof(Config))]
public class NuGetSamples
{
    private class Config : ManualConfig
    {
        public Config()
        {
            AddDiagnoser(MemoryDiagnoser.Default);
            var baseJob = Job.MediumRun;
            AddJob(baseJob.WithNuGet("YamlDotNet", "16.3.0").WithId("YamlDotNet 16.3.0"));
            AddJob(baseJob.WithNuGet("YamlDotNet", "7.0.0").WithId("YamlDotNet 7.0.0"));
        }
    }

    private readonly ISerializer _serializer = new SerializerBuilder().Build();

    [Benchmark]
    public string Serialize() => _serializer.Serialize(new { Name = "AsyncWizard", Version = "1.0.0" });
}

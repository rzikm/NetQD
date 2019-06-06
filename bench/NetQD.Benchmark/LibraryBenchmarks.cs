using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.CsProj;

namespace NetQD.Benchmark
{
	[Config(typeof(Config))]
	public class LibraryBenchmarks
	{
        public class Config : ManualConfig
        {
            public Config()
            {
                var baseJob = Job.MediumRun.With(CsProjCoreToolchain.Current.Value).WithLaunchCount(1).WithIterationCount(10);
                Add(baseJob);
            }
        }

        [Benchmark]
		public QdReal Add()
		{
			QdReal a = 1;
			QdReal sum = 0;
			for (int i = 0; i < 1024; i++)
			{
				sum += a;
			}

			return sum;
		}
	}
}

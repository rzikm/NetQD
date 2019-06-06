using System;
using BenchmarkDotNet.Running;

namespace NetQD.Benchmark
{
	class Program
	{
		static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        }
	}
}

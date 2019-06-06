using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace NetQD.Benchmark
{
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class DdRealCreateAdd
    {
        private static double a = 1;
        private static double b = 2;

        [Benchmark(Baseline = true)]
        public DdReal Current()
        {
            return DdReal.Add(a, b);
        }

        [Benchmark]
        public DdReal Unpack()
        {
            return DdReal.Add_Unpack(a, b);
        }

        [Benchmark]
        public DdReal Hybrid()
        {
            return DdReal.Add_Hybrid(a, b);
        }

        [Benchmark]
        public DdReal OutArgs()
        {
            return DdReal.Add_OutArgs(a, b);
        }

        [Benchmark]
        public DdReal Direct()
        {
            return DdReal.Add_DirectPtr(a, b);
        }

        [Benchmark]
        public DdReal OutToCtor()
        {
            return DdReal.Add_OutToCtor(a, b);
        }

        [Benchmark]
        public DdReal UnpackToCtor()
        {
            return DdReal.Add_UnpackToCtor(a, b);
        }
    }
}

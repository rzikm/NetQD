using System.Runtime.CompilerServices;
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
        public DdReal OutToCtor()
        {
            return Add_OutToCtor(a, b);
        }

        [Benchmark]
        public DdReal UnpackToCtor()
        {
            return Add_UnpackToCtor(a, b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static DdReal Add_OutToCtor(double a, double b)
        {
            TwoSum(a, b, out var sum, out var error);
            return new DdReal(sum, error);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static DdReal Add_UnpackToCtor(double a, double b)
        {
            (double s, double e) = MathHelper.TwoSum(a, b);
            return new DdReal(s, e);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TwoSum(double a, double b, out double sum, out double error)
        {
            sum = a + b;
            double bb = sum - a;
            error = a - (sum - bb) + (b - bb);
        }
    }
}

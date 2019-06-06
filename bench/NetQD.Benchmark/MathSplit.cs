using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace NetQD.Benchmark
{
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class MathSplit
    {
        private static int N = 1024;
        const double tFactor = (1 << 27) + 1;

        [Benchmark(Baseline = true)]
        public double Current()
        {
            double counter = 0;
            double errors = 0;
            for (int i = 0; i < N; i++)
            {
                (double prod, double err) = MathHelper.TwoProd(i, i + 1);
                counter += prod;
                errors += err;
            }

            return counter + errors;
        }

        [Benchmark]
        public double OutArgs()
        {
            double counter = 0;
            double errors = 0;
            for (int i = 0; i < N; i++)
            {
                (double prod, double err) = TwoProd_OutArgs(i, i + 1);
                counter += prod;
                errors += err;
            }

            return counter + errors;
        }

        private static (double prod, double err) TwoProd_OutArgs(double a, double b)
        {
            double product = a * b;
            Split_OutArgs(a, out var ahi, out var alo);
            Split_OutArgs(b, out var bhi, out var blo);
            return (product, ((ahi * bhi - product) + ahi * blo + alo * bhi) + alo * blo);
        }

        private static void Split_OutArgs(double a, out double high, out double low)
        {
            double t = tFactor * a;
            high = t - (t - a);
            low = a - high;
        }

        [Benchmark]
        public double OutArgs_inline()
        {
            double counter = 0;
            double errors = 0;
            for (int i = 0; i < N; i++)
            {
                (double prod, double err) = TwoProd_OutArgs_inline(i, i + 1);
                counter += prod;
                errors += err;
            }

            return counter + errors;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static (double prod, double err) TwoProd_OutArgs_inline(double a, double b)
        {
            double product = a * b;
            Split_OutArgs_inline(a, out var ahi, out var alo);
            Split_OutArgs_inline(b, out var bhi, out var blo);
            return (product, ((ahi * bhi - product) + ahi * blo + alo * bhi) + alo * blo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Split_OutArgs_inline(double a, out double high, out double low)
        {
            double t = tFactor * a;
            high = t - (t - a);
            low = a - high;
        }
    }
}

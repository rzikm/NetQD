using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace NetQD.Benchmark
{
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class MathQuickTwoSum
    {
        private static int N = 16 * 1024;

        [Benchmark]
        public double Hybrid()
        {
            double counter = 0;
            double errors = 0;
            for (int i = 0; i < N; i++)
            {
                counter += QuickTwoSum(i, i + 1, out var err);
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
                QuickTwoSum(i, i + 1, out var sum, out var err);
                counter += sum;
                errors += err;
            }

            return counter + errors;
        }
        
        [Benchmark]
        public double ExplicitStruct()
        {
            double counter = 0;
            double errors = 0;
            for (int i = 0; i < N; i++)
            {
                var s = QuickTwoSumStruct(1, 2);
                counter += s.sum;
                errors += s.error;
            }

            return counter + errors;
        }

        [Benchmark]
        public double ExplicitStruct_Deconstructed()
        {
            double counter = 0;
            double errors = 0;
            for (int i = 0; i < N; i++)
            {
                var (sum, err) = QuickTwoSumStruct(1, 2);
                counter += sum;
                errors += err;
            }

            return counter + errors;
        }

        [Benchmark(Baseline = true)]
        public double Current()
        {
            double counter = 0;
            double errors = 0;
            for (int i = 0; i < N; i++)
            {
                var s = MathHelper.QuickTwoSum(1, 2);
                counter += s.sum;
                errors += s.error;
            }

            return counter + errors;
        }

        [Benchmark]
        public double NamedTuple_Deconstructed()
        {
            double counter = 0;
            double errors = 0;
            for (int i = 0; i < N; i++)
            {
                var (sum, err) = QuickTwoSum_NamedTuple(1, 2);
                counter += sum;
                errors += err;
            }

            return counter + errors;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double sum, double error) QuickTwoSum_NamedTuple(double a, double b)
        {
            var s = a + b;
            var e = b - (s - a);
            return (s, e);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void QuickTwoSum(double a, double b, out double sum, out double error)
        {
            sum = a + b;
            error = b - (sum - a);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double QuickTwoSum(double a, double b, out double error)
        {
            var sum = a + b;
            error = b - (sum - a);
            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Sum QuickTwoSumStruct(double a, double b)
        {
            var s = a + b;
            var e = b - (s - a);
            return new Sum
            {
                sum = s,
                error = e
            };
        }

        public struct Sum
        {
            public double sum;
            public double error;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Deconstruct(out double s, out double e)
            {
                s = sum;
                e = error;
            }
        }
    }
}

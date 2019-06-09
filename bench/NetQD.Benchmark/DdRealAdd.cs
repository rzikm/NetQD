using System;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace NetQD.Benchmark
{
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public unsafe class DdRealAdd
    {
        DdReal a = new DdReal();
        DdReal b = new DdReal();

        [Benchmark(Baseline = true)]
        public DdReal Current()
        {
            return a.Add(b);
        } 

        [Benchmark]
        public DdReal Operator()
        {
            return a + b;
        } 

        [Benchmark]
        public DdReal Add_Norm()
        {
            return Add_Norm(a, b);
        } 

        [Benchmark]
        public DdReal Add_in()
        {
            return Add_in(a, b);
        } 

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static DdReal Add_Norm(DdReal left, DdReal right)
        {
            (double s1, double s2) = MathHelper.TwoSum(left.High, right.High);
            (double t1, double t2) = MathHelper.TwoSum(left.High, right.High);
            s2 += t1;
            (s1, s2) = MathHelper.QuickTwoSum(s1, s2);
            s2 += t2;
            return MathHelper.QuickTwoSumD(s1, s2);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static DdReal Add_in(in DdReal left, in DdReal right)
        {
            (double s1, double s2) = MathHelper.TwoSum(left.High, right.High);
            (double t1, double t2) = MathHelper.TwoSum(left.High, right.High);
            s2 += t1;
            (s1, s2) = MathHelper.QuickTwoSum(s1, s2);
            s2 += t2;
            (s1, s2) = MathHelper.QuickTwoSum(s1, s2);
            return new DdReal(s1, s2);
        }
    }
}

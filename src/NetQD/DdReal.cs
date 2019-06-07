using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetQD
{
	public unsafe struct DdReal
    {
        internal fixed double data[2];

        public DdReal(double x0, double x1)
        {
            data[0] = x0;
            data[1] = x1;
        }

        #region Addition

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DdReal Add(double a, double b)
        {
            DdReal result;
            MathHelper.TwoSum(a, b, result.data);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DdReal Add(double other)
        {
            var (s1, s2) = MathHelper.TwoSum(data[0], other);
            s2 += data[1];
            (s1, s2) = MathHelper.QuickTwoSum(s1, s2);
            return new DdReal(s1, s2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DdReal Add(DdReal other)
        {
            var (s1, s2) = MathHelper.TwoSum(data[0], other.data[0]);
            var (t1, t2) = MathHelper.TwoSum(data[1], other.data[1]);
            s2 += t1;
            (s1, s2) = MathHelper.QuickTwoSum(s1, s2);
            s2 += t2;
            (s1, s2) = MathHelper.QuickTwoSum(s1, s2);
            return new DdReal(s1, s2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DdReal AddSloppy(DdReal other)
        {
            var (s, e) = MathHelper.TwoSum(data[0], other.data[0]);
            e += data[1] + other.data[1];
            (s, e) = MathHelper.QuickTwoSum(s, e);
            return new DdReal(s, e);
        }

        #endregion

        #region Subtraction

        

        #endregion
    }
}

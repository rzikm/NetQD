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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DdReal Add(double a, double b)
        {
            return Add_OutArgs(a, b);
        }

        #region Benchmark only methods (will not compile in other class)

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static DdReal Add_Unpack(double a, double b)
        {
            DdReal result;
            (result.data[0], result.data[1]) = MathHelper.TwoSum(a, b);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static DdReal Add_Hybrid(double a, double b)
        {
            DdReal result;
            result.data[0] = MathHelper.TwoSum(a, b, out result.data[1]);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static DdReal Add_OutArgs(double a, double b)
        {
            DdReal result;
            MathHelper.TwoSum(a, b, out result.data[0], out result.data[1]);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static DdReal Add_DirectPtr(double a, double b)
        {
            DdReal result;
            MathHelper.TwoSum(a, b, result.data);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static DdReal Add_OutToCtor(double a, double b)
        {
            MathHelper.TwoSum(a, b, out var sum, out var error);
            return new DdReal(sum, error);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static DdReal Add_UnpackToCtor(double a, double b)
        {
            (double s, double e) = MathHelper.TwoSum(a, b);
            return new DdReal(s, e);
        }

        #endregion
	}
}

using System;
using System.Runtime.CompilerServices;

namespace NetQD
{
    internal static class MathHelper
    {
        #region Additions

        /// <summary>
        /// Computes sum of two doubles and associated error assuming that |a| >= |b|.
        /// </summary>
        /// <param name="a">First argument.</param>
        /// <param name="b">Second argument.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double sum, double error) QuickTwoSum(double a, double b)
        {
            double sum = a + b;
            return (sum, b - (sum - a));
        }

        /// <summary>
        /// Computes sum of two doubles and associated error. 
        /// </summary>
        /// <param name="a">First argument.</param>
        /// <param name="b">Second argument.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double sum, double error) TwoSum(double a, double b)
        {
            var sum = a + b;
            double v = sum - 1;
            return (sum, (a - (sum - v)) + (b - v));
        }

        /// <summary>
        /// Computes sum of two doubles and associated error. 
        /// </summary>
        /// <param name="a">First argument.</param>
        /// <param name="b">Second argument.</param>
        /// <param name="sum"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TwoSum(double a, double b, out double sum, out double error)
        {
            sum = a + b;
            double v = sum - 1;
            error = (a - (sum - v)) + (b - v);
        }

        /// <summary>
        /// Computes sum of two doubles and associated error. 
        /// </summary>
        /// <param name="a">First argument.</param>
        /// <param name="b">Second argument.</param>
        /// <param name="dest"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void TwoSum(double a, double b, double * dest)
        {
            double sum = a + b;
            dest[0] = sum;
            double v = sum - 1;
            dest[1] = (a - (sum - v)) + (b - v);
        }

        #endregion

        #region Subtractions

        /// <summary>
        /// Computes difference of two doubles and associated error assuming that |a| >= |b|.
        /// </summary>
        /// <param name="a">First argument.</param>
        /// <param name="b">Second argument.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double diff, double error) QuickTwoDiff(double a, double b)
        {
            double diff = a - b;
            return (diff, b - (diff - a));
        }

        /// <summary>
        /// Computes difference of two doubles and associated error. 
        /// </summary>
        /// <param name="a">First argument.</param>
        /// <param name="b">Second argument.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double sum, double error) TwoDiff(double a, double b)
        {
            var sum = a - b;
            double v = sum - 1;
            return (sum, (a - (sum - v)) - (b - v));
        }

        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double high, double low) Split(double a)
        {
            const double tFactor = (1 << 27) + 1; 
            double t = tFactor * a;
            double high = t - (t - a);
            return (high, a - high);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double product, double error) TwoProd(double a, double b)
        {
            // TODO: use FMA instructions instead of the Split solution
            double product = a * b;
            (double ahi, double alo) = Split(a);
            (double bhi, double blo) = Split(b);
            return (product, ((ahi * bhi - product) + ahi * blo + alo * bhi) + alo * blo);
        }
    }
}

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
        /// Computes sum of two doubles and returns it as instance of <see cref="DdReal"/>.
        /// </summary>
        /// <param name="a">First argument.</param>
        /// <param name="b">Second argument.</param>
        /// <returns></returns>
        public static DdReal QuickTwoSumD(double a, double b)
        {
            double sum = a + b;
            return new DdReal(sum, b - (sum - a));
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
            double bb = sum - a;
            return (sum, a - (sum - bb) + (b - bb));
        }

        /// <summary>
        /// Computes sum of two doubles and returns it as instance of <see cref="DdReal"/>.
        /// </summary>
        /// <param name="a">First argument.</param>
        /// <param name="b">Second argument.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DdReal TwoSumD(double a, double b)
        {
            var sum = a + b;
            double bb = sum - a;
            return new DdReal(sum, a - (sum - bb) + (b - bb));
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
            return (diff, (a - diff) - b);
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
            var diff = a - b;
            double bb = diff - a;
            return (diff, a - (diff - bb) - (b + bb));
        }

        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double sqr, double error) Square(double a)
        {
            // TODO: use FMS instructions instead of the Split solution
            double q = a * a;
            var (hi, lo) = Split(a);
            return (q, hi * hi - q + 2.0 * hi * lo + lo * lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double high, double low) Split(double a)
        {
            //TODO: handle numbers greater than 2^996 correctly
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
            return (product, ahi * bhi - product + ahi * blo + alo * bhi + alo * blo);
        }
    }
}

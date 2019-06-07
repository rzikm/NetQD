﻿using System;
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
            double v = sum - 1;
            return (sum, a - (sum - v) + (b - v));
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
            double v = sum - 1;
            return new DdReal(sum, a - (sum - v) + (b - v));
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
            error = a - (sum - v) + (b - v);
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
            return (sum, a - (sum - v) - (b - v));
        }

        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double sqr, double error) Square(double a)
        {
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
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DdReal Renormalize(double s1, double s2, double t1, double t2)
        {
            s2 += t1;
            (s1, s2) = MathHelper.QuickTwoSum(s1, s2);
            s2 += t2;
            return MathHelper.QuickTwoSumD(s1, s2);
        }
    }
}

using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace NetQD
{
    /// <summary>
    ///     Represents a floating number with quad-double precision (256-bits)
    /// </summary>
    public struct QdReal
        : IComparable<QdReal>, IComparable, IEquatable<QdReal>, IConvertible, IFormattable
    {
        public static QdReal Zero => new QdReal(0);
        public static QdReal One => new QdReal(1);

        internal readonly double x0;
        internal readonly double x1;
        internal readonly double x2;
        internal readonly double x3;

        public QdReal(double x0, double x1 = 0, double x2 = 0, double x3 = 0)
        {
            this.x0 = x0;
            this.x1 = x1;
            this.x2 = x2;
            this.x3 = x3;
        }

        #region Addition

        public static unsafe QdReal operator +(in QdReal left, in QdReal right)
        {
            int i, j, k;
            double s, t;
            double u, v;
            var x = stackalloc double[4];

            i = j = k = 0;
            if (Math.Abs(left.x0) > Math.Abs(right.x0))
            {
                u = left[i++];
            }
            else
            {
                u = right[j++];
            }

            if (Math.Abs(left[i]) > Math.Abs(right[j]))
            {
                v = left[i++];
            }
            else
            {
                v = right[j++];
            }

            (u, v) = MathHelper.QuickTwoSum(u, v);

            while (k < 4)
            {
                if (i >= 4 && j >= 4)
                {
                    x[k] = u;
                    if (k < 3)
                    {
                        x[++k] = v;
                    }

                    break;
                }

                if (i >= 4)
                {
                    t = right[j++];
                }
                else if (j >= 4)
                {
                    t = left[i++];
                }
                else if (Math.Abs(left[i]) > Math.Abs(right[j]))
                {
                    t = left[i++];
                }
                else
                {
                    t = right[j++];
                }

                s = QuickThreeAccum(ref u, ref v, ref t);

                if (s != 0.0)
                {
                    x[k++] = s;
                }
            }

            for (k = i; k < 4; k++)
                x[3] += left[k];
            for (k = j; k < 4; k++)
                x[3] += right[k];

            return Renormalize(x[0], x[1], x[2], x[3]);
        }

        public static QdReal operator +(in QdReal left, in DdReal right)
        {
            var (s0, t0) = MathHelper.TwoSum(left.x0, right.x0);
            var (s1, t1) = MathHelper.TwoSum(left.x1, right.x1);

            (s1, t0) = MathHelper.TwoSum(s1, t0);

            var s2 = left.x2;
            ThreeSum(ref s2, ref t0, ref t1);

            double s3;
            (s3, t0) = MathHelper.TwoSum(t0, left.x3);
            t0 += t1;

            return Renormalize(s0, s1, s2, s3, t0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QdReal operator +(in DdReal left, in QdReal right) => right + left;

        public static QdReal operator +(in QdReal left, double right)
        {
            double c0, c1, c2, c3;
            double e;

            (c0, e) = MathHelper.TwoSum(left.x0, right);
            (c1, e) = MathHelper.TwoSum(left.x1, e);
            (c2, e) = MathHelper.TwoSum(left.x2, e);
            (c3, e) = MathHelper.TwoSum(left.x3, e);

            return Renormalize(c0, c1, c2, c3, e);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QdReal operator +(double left, in QdReal right) => right + left;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QdReal Add(double left, double right) => new QdReal(left) + right;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public QdReal Add(in QdReal other) => this + other;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public QdReal Add(in DdReal other) => this + other;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public QdReal Add(double other) => this + other;

        public QdReal AddSloppy(in QdReal other)
        {
            double s0, s1, s2, s3;
            double t0, t1, t2, t3;

            (s0, t0) = MathHelper.TwoSum(x0, other.x0);
            (s1, t1) = MathHelper.TwoSum(x1, other.x1);
            (s2, t2) = MathHelper.TwoSum(x2, other.x2);
            (s3, t3) = MathHelper.TwoSum(x3, other.x3);

            (s1, t0) = MathHelper.TwoSum(s1, t0);
            ThreeSum(ref s2, ref t0, ref t1);
            ThreeSum2(ref s3, ref t0, ref t2);
            t0 = t0 + t1 + t3;

            return Renormalize(s0, s1, s2, s3, t0);
        }

        #endregion

        #region Subtraction

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QdReal operator -(in QdReal value)
            => new QdReal(-value.x0, -value.x1, -value.x2, -value.x3);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QdReal operator -(in QdReal left, in QdReal right) => left + -right;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QdReal operator -(in QdReal left, in DdReal right) => left + -right;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QdReal operator -(in DdReal left, in QdReal right) => left + -right;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QdReal operator -(in QdReal left, double right) => left + -right;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QdReal operator -(double left, in QdReal right) => left + -right;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QdReal Subtract(double left, double right) => new QdReal(left) - right;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public QdReal Subtract(in QdReal other) => this - other;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public QdReal Subtract(in DdReal other) => this - other;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public QdReal Subtract(double other) => this - other;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public QdReal SubtractSloppy(in QdReal other) => AddSloppy(-other);

        #endregion

        #region Multiplication

        public static QdReal operator *(in QdReal left, in QdReal right)
        {
            double p0, p1, p2, p3, p4, p5;
            double q0, q1, q2, q3, q4, q5;
            double p6, p7, p8, p9;
            double q6, q7, q8, q9;
            double r0, r1;
            double t0, t1;
            double s0, s1, s2;

            (p0, q0) = MathHelper.TwoProd(left.x0, right.x0);

            (p1, q1) = MathHelper.TwoProd(left.x0, right.x1);
            (p2, q2) = MathHelper.TwoProd(left.x1, right.x0);

            (p3, q3) = MathHelper.TwoProd(left.x0, right.x2);
            (p4, q4) = MathHelper.TwoProd(left.x1, right.x1);
            (p5, q5) = MathHelper.TwoProd(left.x2, right.x0);

            /* Start Accumulation */
            ThreeSum(ref p1, ref p2, ref q0);

            /* Six-Three Sum  of p2, q1, q2, p3, p4, p5. */
            ThreeSum(ref p2, ref q1, ref q2);
            ThreeSum(ref p3, ref p4, ref p5);
            /* compute (s0, s1, s2) = (p2, q1, q2) + (p3, p4, p5). */
            (s0, t0) = MathHelper.TwoSum(p2, p3);
            (s1, t1) = MathHelper.TwoSum(q1, p4);
            s2 = q2 + p5;
            (s1, t0) = MathHelper.TwoSum(s1, t0);
            s2 += t0 + t1;

            /* O(eps^3) order terms */
            (p6, q6) = MathHelper.TwoProd(left.x0, right.x3);
            (p7, q7) = MathHelper.TwoProd(left.x1, right.x2);
            (p8, q8) = MathHelper.TwoProd(left.x2, right.x1);
            (p9, q9) = MathHelper.TwoProd(left.x3, right.x0);

            /* Nine-Two-Sum of q0, s1, q3, q4, q5, p6, p7, p8, p9. */
            (q0, q3) = MathHelper.TwoSum(q0, q3);
            (q4, q5) = MathHelper.TwoSum(q4, q5);
            (p6, p7) = MathHelper.TwoSum(p6, p7);
            (p8, p9) = MathHelper.TwoSum(p8, p9);
            /* Compute (t0, t1) = (q0, q3) + (q4, q5). */
            (t0, t1) = MathHelper.TwoSum(q0, q4);
            t1 += q3 + q5;
            /* Compute (r0, r1) = (p6, p7) + (p8, p9). */
            (r0, r1) = MathHelper.TwoSum(p6, p8);
            r1 += p7 + p9;
            /* Compute (q3, q4) = (t0, t1) + (r0, r1). */
            (q3, q4) = MathHelper.TwoSum(t0, r0);
            q4 += t1 + r1;
            /* Compute (t0, t1) = (q3, q4) + s1. */
            (t0, t1) = MathHelper.TwoSum(q3, s1);
            t1 += q4;

            /* O(eps^4) terms -- Nine-One-Sum */
            t1 += left.x1 * right.x3 +
                left.x2 * right.x2 +
                left.x3 * right.x1 +
                q6 +
                q7 +
                q8 +
                q9 +
                s2;

            return Renormalize(p0, p1, s0, t0, t1);
        }

        public static QdReal operator *(in QdReal left, in DdReal right)
        {
            double p0, p1, p2, p3, p4;
            double q0, q1, q2, q3, q4;
            double s0, s1, s2;
            double t0, t1;

            (p0, q0) = MathHelper.TwoProd(left.x0, right.x0);
            (p1, q1) = MathHelper.TwoProd(left.x0, right.x1);
            (p2, q2) = MathHelper.TwoProd(left.x1, right.x0);
            (p3, q3) = MathHelper.TwoProd(left.x1, right.x1);
            (p4, q4) = MathHelper.TwoProd(left.x2, right.x0);

            ThreeSum(ref p1, ref p2, ref q0);

            ThreeSum(ref p2, ref p3, ref p4);
            (q1, q2) = MathHelper.TwoSum(q1, q2);
            (s0, t0) = MathHelper.TwoSum(p2, q1);
            (s1, t1) = MathHelper.TwoSum(p3, q2);
            (s1, t0) = MathHelper.TwoSum(s1, t0);
            s2 = t0 + t1 + p4;
            p2 = s0;

            p2 = left.x2 * right.x0 + left.x3 * right.x1 + q3 + q4;
            ThreeSum2(ref p3, ref q0, ref s1);
            p4 = q0 + s2;

            return Renormalize(p0, p1, p2, p3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QdReal operator *(in DdReal left, in QdReal right) => right * left;

        public static QdReal operator *(in QdReal left, double right)
        {
            double p0, p1, p2, p3;
            double q0, q1, q2;
            double s0, s1, s2, s3, s4;

            (p0, q0) = MathHelper.TwoProd(left.x0, right);
            (p1, q1) = MathHelper.TwoProd(left.x2, right);
            (p2, q2) = MathHelper.TwoProd(left.x2, right);
            p3 = left.x3 * right;

            s0 = p0;

            (s1, s2) = MathHelper.TwoSum(q0, p1);

            ThreeSum(ref s2, ref q1, ref p2);
            ThreeSum2(ref q1, ref q2, ref p3);
            s3 = q1;
            s4 = q2 + p2;

            return Renormalize(s0, s1, s2, s3, s4);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QdReal operator *(double left, in QdReal right) => right * left;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QdReal Multiply(double left, double right) => new QdReal(left) * right;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public QdReal Multiply(in QdReal other) => this * other;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public QdReal Multiply(in DdReal other) => this * other;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public QdReal Multiply(double other) => this * other;

        public QdReal MultiplySloppy(in QdReal other)
        {
            double p0, p1, p2, p3, p4, p5;
            double q0, q1, q2, q3, q4, q5;
            double t0, t1;
            double s0, s1, s2;

            (p0, q0) = MathHelper.TwoProd(x0, other.x0);

            (p1, q1) = MathHelper.TwoProd(x0, other.x1);
            (p2, q2) = MathHelper.TwoProd(x1, other.x0);

            (p3, q3) = MathHelper.TwoProd(x0, other.x2);
            (p4, q4) = MathHelper.TwoProd(x1, other.x1);
            (p5, q5) = MathHelper.TwoProd(x2, other.x0);

            /* Start Accumulation */
            ThreeSum(ref p1, ref p2, ref q0);

            /* Six-Three Sum  of p2, q1, q2, p3, p4, p5. */
            ThreeSum(ref p2, ref q1, ref q2);
            ThreeSum(ref p3, ref p4, ref p5);
            /* compute (s0, s1, s2) = (p2, q1, q2) + (p3, p4, p5). */
            (s0, t0) = MathHelper.TwoSum(p2, p3);
            (s1, t1) = MathHelper.TwoSum(q1, p4);
            s2 = q2 + p5;
            (s1, t0) = MathHelper.TwoSum(s1, t0);
            s2 += t0 + t1;

            /* O(eps^3) order terms */
            s1 += x0 * other.x3 + x1 * other.x2 + x2 * other.x1 + x3 * other.x0 + q0 + q3 + q4 + q5;
            return Renormalize(p0, p1, s0, s1, s2);
        }

        #endregion

        #region Division

        public static QdReal operator /(in QdReal left, in QdReal right)
        {
            double q0, q1, q2, q3;

            QdReal r;

            q0 = left.x0 / right.x0;
            r = left - right * q0;

            q1 = r.x0 / right.x0;
            r -= right * q1;

            q2 = r.x0 / right.x0;
            r -= right * q2;

            q3 = r.x0 / right.x0;

            r -= right * q3;
            var q4 = r.x0 / right.x0;

            return Renormalize(q0, q1, q2, q3, q4);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QdReal operator /(in QdReal left, in DdReal right) => left / (QdReal) right;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QdReal operator /(in DdReal left, in QdReal right) => (QdReal) left / right;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QdReal operator /(in QdReal left, double right)
        {
            /* Strategy:  compute approximate quotient using high order
               doubles, and then correct it 3 times using the remainder.
               (Analogous to long division.)                             */
            double t0, t1;
            double q0, q1, q2, q3;
            QdReal r;

            q0 = left.x0 / right; /* approximate quotient */

            /* Compute the remainder  a - q0 * right */
            (t0, t1) = MathHelper.TwoProd(q0, right);
            r = left - new DdReal(t0, t1);

            /* Compute the first correction */
            q1 = r[0] / right;
            (t0, t1) = MathHelper.TwoProd(q1, right);
            r -= new DdReal(t0, t1);

            /* Second correction to the quotient. */
            q2 = r[0] / right;
            (t0, t1) = MathHelper.TwoProd(q2, right);
            r -= new QdReal(t0, t1);

            /* Final correction to the quotient. */
            q3 = r[0] / right;

            return Renormalize(q0, q1, q2, q3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QdReal operator /(double left, in QdReal right) => (QdReal) left / right;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QdReal Divide(double left, double right) => new QdReal(left) / right;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public QdReal Divide(in QdReal other) => this / other;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public QdReal Divide(in DdReal other) => this / other;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public QdReal Divide(double other) => this / other;

        public QdReal DivideSloppy(in QdReal other)
        {
            double q0, q1, q2, q3;

            QdReal r;

            q0 = x0 / other.x0;
            r = this - other * q0;

            q1 = r[0] / other.x0;
            r -= other * q1;

            q2 = r[0] / other.x0;
            r -= other * q2;

            q3 = r[0] / other.x0;

            return Renormalize(q0, q1, q2, q3);
        }

        #endregion

        #region Other math operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsNaN()
        {
            return double.IsNaN(x0) | double.IsNaN(x1) | double.IsNaN(x2) | double.IsNaN(x3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsInfinity()
        {
            return double.IsInfinity(x0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsFinite()
        {
            return !(double.IsInfinity(x0) | double.IsNaN(x0));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsPositiveInfinity()
        {
            return double.IsPositiveInfinity(x0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsNegativeInfinity()
        {
            return double.IsNegativeInfinity(x0);
        }

        #endregion

        #region Conversions

        // To QdReal

        public static implicit operator QdReal(DdReal value) => new QdReal(value.x0, value.x1);

        // TODO: dont throw away the truncated data
        public static explicit operator QdReal(decimal value) => new QdReal((double) value);

        public static implicit operator QdReal(double value) => new QdReal(value);

        public static implicit operator QdReal(float value) => new QdReal(value);

        public static implicit operator QdReal(ulong value) => new QdReal(value);

        public static implicit operator QdReal(long value) => new QdReal(value);

        public static implicit operator QdReal(uint value) => new QdReal(value);

        public static implicit operator QdReal(int value) => new QdReal(value);

        public static implicit operator QdReal(short value) => new QdReal(value);

        public static implicit operator QdReal(ushort value) => new QdReal(value);

        public static implicit operator QdReal(byte value) => new QdReal(value);

        public static implicit operator QdReal(sbyte value) => new QdReal(value);

        public static implicit operator QdReal(char value) => new QdReal(value);

        // From QdReal

        public static explicit operator double(QdReal value) => value.x0;

        public static explicit operator decimal(QdReal value)
            => (decimal)value.x0 + (decimal)value.x1 + (decimal)value.x2 + (decimal)value.x3;

        public static explicit operator float(QdReal value) => (float) value.x0;

        public static explicit operator ulong(QdReal value) => (ulong) value.x0;

        public static explicit operator long(QdReal value) => (long) value.x0;

        public static explicit operator uint(QdReal value) => (uint) value.x0;

        public static explicit operator int(QdReal value) => (int) value.x0;

        public static explicit operator short(QdReal value) => (short) value.x0;

        public static explicit operator ushort(QdReal value) => (ushort) value.x0;

        public static explicit operator byte(QdReal value) => (byte) value.x0;

        public static explicit operator sbyte(QdReal value) => (sbyte) value.x0;

        public static explicit operator char(QdReal value) => (char) value.x0;

        TypeCode IConvertible.GetTypeCode() => TypeCode.Double;

        bool IConvertible.ToBoolean(IFormatProvider provider)
            => throw new InvalidCastException("Cannot cast QdReal to bool");

        byte IConvertible.ToByte(IFormatProvider provider) => (byte) this;

        char IConvertible.ToChar(IFormatProvider provider) => (char) this;

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
            => throw new InvalidCastException("Cannot cast QdReal to DateTime");

        decimal IConvertible.ToDecimal(IFormatProvider provider) => (decimal) this;

        double IConvertible.ToDouble(IFormatProvider provider) => (double) this;

        short IConvertible.ToInt16(IFormatProvider provider) => (short) this;

        int IConvertible.ToInt32(IFormatProvider provider) => (int) this;

        long IConvertible.ToInt64(IFormatProvider provider) => (long) this;

        sbyte IConvertible.ToSByte(IFormatProvider provider) => (sbyte) this;

        float IConvertible.ToSingle(IFormatProvider provider) => (float) this;

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            if (conversionType == typeof(DdReal))
                return (DdReal)this;

            if (conversionType == typeof(object))
                return this;

            if (Type.GetTypeCode(conversionType) != TypeCode.Object)
                return Convert.ChangeType(this, Type.GetTypeCode(conversionType), provider);

            throw new InvalidCastException($"Converting type \"{typeof(DdReal)}\" to type \"{conversionType.Name}\" is not supported.");
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider) => (ushort) this;

        uint IConvertible.ToUInt32(IFormatProvider provider) => (uint) this;

        ulong IConvertible.ToUInt64(IFormatProvider provider) => (ulong) this;

        #endregion

        #region Relational Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(QdReal left, QdReal right) => left.Equals(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(QdReal left, QdReal right) => !left.Equals(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(QdReal left, QdReal right) => left.CompareTo(right) < 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(QdReal left, QdReal right) => left.CompareTo(right) > 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(QdReal left, QdReal right) => left.CompareTo(right) <= 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(QdReal left, QdReal right) => left.CompareTo(right) >= 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(QdReal other)
        {
            var cmp = x0.CompareTo(other.x0);
            if (cmp != 0)
            {
                return cmp;
            }

            cmp = x1.CompareTo(other.x1);
            if (cmp != 0)
            {
                return cmp;
            }

            cmp = x2.CompareTo(other.x2);
            if (cmp != 0)
            {
                return cmp;
            }

            return x3.CompareTo(other.x3);
        }

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return 1;
            }

            return obj is QdReal other
                ? CompareTo(other)
                : throw new ArgumentException($"Object must be of type {nameof(QdReal)}");
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(QdReal other)
            => x0.Equals(other.x0) &&
                x1.Equals(other.x1) &&
                x2.Equals(other.x2) &&
                x3.Equals(other.x3);

        public override bool Equals(object obj) => obj is QdReal other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = x0.GetHashCode();
                hashCode = (hashCode * 397) ^ x1.GetHashCode();
                hashCode = (hashCode * 397) ^ x2.GetHashCode();
                hashCode = (hashCode * 397) ^ x3.GetHashCode();
                return hashCode;
            }
        }

        #endregion

        #region Parsing and printing

        public static QdReal Parse(string s)
        {
            if (TryParse(s, out var value))
            {
                return value;
            }

            throw new FormatException();
        }

        public static bool TryParse(string s, out QdReal value)
        {
            // TODO: support for IFormatProvider as double.TryParse does

            int p = 0;
            int sign = 0;
            int point = -1;
            int nd = 0;
            int e = 0;
            bool done = false;
            QdReal r = 0.0;
            value = r;

            while (!done && p != s.Length)
            {
                char ch = s[p++];
                if (char.IsDigit(ch))
                {
                    int d = ch - '0';
                    r = r * 10 + d;
                    nd++;
                    continue;
                }

                switch (ch)
                {
                    case '.':
                        if (point >= 0)
                            return false;
                        point = nd;
                        break;

                    case '-':
                    case '+':
                        if (sign != 0 || nd > 0)
                            return false;
                        sign = (ch == '-') ? -1 : 1;
                        break;

                    case 'E':
                    case 'e':
                        if (!int.TryParse(s.Substring(p), out e))
                            return false;
                        done = true;
                        break;

                    default:
                        return false;
                }
            }

            if (point >= 0)
            {
                e -= nd - point;
            }

            if (e != 0)
            {
                // TODO: implement QdReal.Pow
                r *= Math.Pow(10, e);
            }

            value = (sign == -1) ? -r : r;
            return p > 0;
        }

        public override string ToString() => ToString("G", CultureInfo.CurrentCulture);

        public string ToString(IFormatProvider provider)
        {
            return ToString("G", provider);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (formatProvider == null)
            {
                formatProvider = CultureInfo.CurrentCulture;
            }

            // TODO: use all members for formatting
            return x0.ToString(format, formatProvider);
        }

        #endregion

        public void Deconstruct(out double x0, out double x1, out double x2, out double x3)
        {
            x0 = this.x0;
            x1 = this.x1;
            x2 = this.x2;
            x3 = this.x3;
        }

        private static QdReal Renormalize(double c0, double c1, double c2, double c3, double c4)
        {
            if (double.IsInfinity(c0))
            {
                return new QdReal(c0, c1, c2, c3);
            }

            double s0;
            (s0, c4) = MathHelper.QuickTwoSum(c3, c4);
            (s0, c3) = MathHelper.QuickTwoSum(c2, s0);
            (s0, c2) = MathHelper.QuickTwoSum(c1, s0);
            (c0, c1) = MathHelper.QuickTwoSum(c0, s0);

            s0 = c0;
            var s1 = c1;
            double s2 = 0;
            double s3 = 0;

            (s0, s1) = MathHelper.QuickTwoSum(c0, c1);
            if (s1 != 0.0)
            {
                (s1, s2) = MathHelper.QuickTwoSum(s1, c2);
                if (s2 != 0.0)
                {
                    (s2, s3) = MathHelper.QuickTwoSum(s2, c3);
                    if (s3 != 0.0)
                    {
                        s3 += c4;
                    }
                    else
                    {
                        s2 += c4;
                    }
                }
                else
                {
                    (s1, s2) = MathHelper.QuickTwoSum(s1, c3);
                    if (s2 != 0.0)
                    {
                        (s2, s3) = MathHelper.QuickTwoSum(s2, c4);
                    }
                    else
                    {
                        (s1, s2) = MathHelper.QuickTwoSum(s1, c4);
                    }
                }
            }
            else
            {
                (s0, s1) = MathHelper.QuickTwoSum(s0, c2);
                if (s1 != 0.0)
                {
                    (s1, s2) = MathHelper.QuickTwoSum(s1, c3);
                    if (s2 != 0.0)
                    {
                        (s2, s3) = MathHelper.QuickTwoSum(s2, c4);
                    }
                    else
                    {
                        (s1, s2) = MathHelper.QuickTwoSum(s1, c4);
                    }
                }
                else
                {
                    (s0, s1) = MathHelper.QuickTwoSum(s0, c3);
                    if (s1 != 0.0)
                    {
                        (s1, s2) = MathHelper.QuickTwoSum(s1, c4);
                    }
                    else
                    {
                        (s0, s1) = MathHelper.QuickTwoSum(s0, c4);
                    }
                }
            }

            return new QdReal(s0, s1, s2, s3);
        }

        private static QdReal Renormalize(double c0, double c1, double c2, double c3)
        {
            if (double.IsInfinity(c0))
            {
                return new QdReal(c0, c1, c2, c3);
            }

            double s0;
            (s0, c3) = MathHelper.QuickTwoSum(c2, c3);
            (s0, c2) = MathHelper.QuickTwoSum(c1, s0);
            (c0, c1) = MathHelper.QuickTwoSum(c0, s0);

            s0 = c0;
            var s1 = c1;
            double s2 = 0;
            double s3 = 0;
            if (s1 != 0.0)
            {
                (s1, s2) = MathHelper.QuickTwoSum(s1, c2);
                if (s2 != 0.0)
                {
                    (s2, s3) = MathHelper.QuickTwoSum(s2, c3);
                }
                else
                {
                    (s1, s2) = MathHelper.QuickTwoSum(s1, c3);
                }
            }
            else
            {
                (s0, s1) = MathHelper.QuickTwoSum(s0, c2);
                if (s1 != 0.0)
                {
                    (s1, s2) = MathHelper.QuickTwoSum(s1, c3);
                }
                else
                {
                    (s0, s1) = MathHelper.QuickTwoSum(s0, c3);
                }
            }

            return new QdReal(s0, s1, s2, s3);
        }

        private static QdReal RenormalizeQuick(double c0, double c1, double c2, double c3,
            double c4)
        {
            double t0, t1, t2, t3;
            double s;
            (s, t3) = MathHelper.QuickTwoSum(c3, c4);
            (s, t2) = MathHelper.QuickTwoSum(c2, s);
            (s, t1) = MathHelper.QuickTwoSum(c1, s);
            (c0, t0) = MathHelper.QuickTwoSum(c0, s);

            (s, t2) = MathHelper.QuickTwoSum(t2, t3);
            (s, t1) = MathHelper.QuickTwoSum(t1, s);
            (c1, t0) = MathHelper.QuickTwoSum(t0, s);

            (s, t1) = MathHelper.QuickTwoSum(t1, t2);
            (c2, t0) = MathHelper.QuickTwoSum(t0, s);

            c3 = t0 + t1;

            return new QdReal(c0, c1, c2, c3);
        }

        private static void ThreeSum(ref double a, ref double b, ref double c)
        {
            double t1, t2, t3;
            (t1, t2) = MathHelper.TwoSum(a, b);
            (a, t3) = MathHelper.TwoSum(c, t1);
            (b, c) = MathHelper.TwoSum(t2, t3);
        }

        private static void ThreeSum2(ref double a, ref double b, ref double c)
        {
            double t1, t2, t3;
            (t1, t2) = MathHelper.TwoSum(a, b);
            (a, t3) = MathHelper.TwoSum(c, t1);
            b = t2 + t3;
        }

        private static double QuickThreeAccum(ref double a, ref double b, ref double c)
        {
            double s;
            bool za, zb;

            (s, b) = MathHelper.TwoSum(b, c);
            (s, a) = MathHelper.TwoSum(a, s);

            za = a != 0.0;
            zb = b != 0.0;

            if (za & zb)
            {
                return s;
            }

            if (!zb)
            {
                b = a;
                a = s;
            }
            else
            {
                a = s;
            }

            return 0.0;
        }

        internal double this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                switch (index)
                {
                    case 0:
                        return x0;
                    case 1:
                        return x1;
                    case 2:
                        return x2;
                    case 3:
                        return x3;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }
    }
}

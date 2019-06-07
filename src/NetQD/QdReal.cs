using System;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;

namespace NetQD
{
    /// <summary>
    ///     Represents a floating number with quad-double precision (256-bits)
    /// </summary>
    public struct QdReal
        : IComparable<QdReal>, IComparable, IEquatable<QdReal>, IConvertible
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
            double* x = stackalloc double[4];

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
                if (i >= 4 && j > 4)
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
            {
                x[3] += left[k];
            }
            for (k = j; k < 4; k++)
            {
                x[3] += right[k];
            }

            return Renormalize(x[0], x[1], x[2], x[3]);
        }

        public static QdReal operator +(in QdReal left, in DdReal right)
        {
            var (s0, t0) = MathHelper.TwoSum(left.x0, right.x0);
            var (s1, t1) = MathHelper.TwoSum(left.x1, right.x1);

            (s1, t0) = MathHelper.TwoSum(s1, t0);

            double s2 = left.x2;
            ThreeSum(ref s2, ref t0, ref t1);

            double s3;
            (s3, t0) = MathHelper.TwoSum(t0, left.x3);
            t0 += t1;

            return Renormalize(s0, s1, s2, s3, t0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QdReal operator +(in DdReal left, in QdReal right)
        {
            return right + left;
        }

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
        public static QdReal operator +(double left, in QdReal right)
        {
            return right + left;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public QdReal Add(in QdReal other)
        {
            return this + other;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public QdReal Add(in DdReal other)
        {
            return this + other;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public QdReal Add(double other)
        {
            return this + other;
        }

        public QdReal AddSlopy(in QdReal other)
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

        #endregion

        #region Multiplication

        #endregion

        #region Division

        #endregion

        #region Conversions

        public static implicit operator QdReal(DdReal value) => new QdReal(value.x0, value.x1);

        public static implicit operator QdReal(double value) => new QdReal(value);

        public static implicit operator QdReal(float value) => new QdReal(value);

        public static explicit operator double(QdReal value) => value.x0;

        public static explicit operator decimal(QdReal value)
            => (decimal) value.x0 + (decimal) value.x1;

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
            => throw new NotSupportedException("Conversion to arbitrary type is not supported");

        ushort IConvertible.ToUInt16(IFormatProvider provider) => (ushort) this;

        uint IConvertible.ToUInt32(IFormatProvider provider) => (uint) this;

        ulong IConvertible.ToUInt64(IFormatProvider provider) => (ulong) this;

        public string ToString(IFormatProvider provider) => ToString();

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

            za = (a != 0.0);
            zb = (b != 0.0);

            if (za & zb)
                return s;

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

        private double this[int index]
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
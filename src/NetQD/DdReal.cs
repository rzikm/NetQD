using System;
using System.Runtime.CompilerServices;

namespace NetQD
{
    /// <summary>
    ///     Represents a floating number with double-double precision (128-bits)
    /// </summary>
    public struct DdReal : IComparable<DdReal>, IComparable, IEquatable<DdReal>, IConvertible
    {
        public static DdReal Zero => new DdReal(0);
        public static DdReal One => new DdReal(1);

        internal readonly double x0;
        internal readonly double x1;

        public double High => x0;
        public double Low => x1;

        public DdReal(double x0, double x1 = 0)
        {
            this.x0 = x0;
            this.x1 = x1;
        }

        #region Addition

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DdReal operator +(DdReal left, DdReal right)
        {
            var (s1, s2) = MathHelper.TwoSum(left.x0, right.x0);
            var (t1, t2) = MathHelper.TwoSum(left.x1, right.x1);
            return Renormalize(s1, s2, t1, t2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DdReal operator +(DdReal left, double right)
        {
            var (s1, s2) = MathHelper.TwoSum(left.x0, right);
            return MathHelper.QuickTwoSumD(s1, s2 + left.x1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DdReal operator +(double left, DdReal right) => right + left;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DdReal Add(double a, double b) => MathHelper.TwoSumD(a, b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DdReal Add(double other) => this + other;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DdReal Add(DdReal other) => this + other;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DdReal AddSloppy(DdReal other)
        {
            var (s, e) = MathHelper.TwoSum(x0, other.x0);
            return MathHelper.QuickTwoSumD(s, e + x1 + other.x1);
        }

        #endregion

        #region Subtraction

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DdReal operator -(DdReal value) => new DdReal(-value.x0, -value.x1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DdReal operator -(DdReal left, DdReal right)
        {
            var (s1, s2) = MathHelper.TwoDiff(left.x0, right.x0);
            var (t1, t2) = MathHelper.TwoDiff(left.x1, right.x1);
            return Renormalize(s1, s2, t1, t2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DdReal operator -(DdReal left, double right)
        {
            var (s1, s2) = MathHelper.TwoDiff(left.x0, right);
            return MathHelper.QuickTwoSumD(s1, s2 + left.x1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DdReal operator -(double left, DdReal right)
        {
            var (s1, s2) = MathHelper.TwoDiff(left, right.x0);
            return MathHelper.QuickTwoSumD(s1, s2 - right.x1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DdReal Subtract(double a, double b)
        {
            var (d, e) = MathHelper.TwoDiff(a, b);
            return new DdReal(d, e);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DdReal Subtract(double other) => this - other;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DdReal Subtract(DdReal other) => this - other;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DdReal SubtractSloppy(DdReal other)
        {
            var (s, e) = MathHelper.TwoDiff(x0, other.x0);
            return MathHelper.QuickTwoSumD(s, e + x1 - other.x1);
        }

        #endregion

        #region Multiplication

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DdReal operator *(DdReal left, DdReal right)
        {
            var (p1, p2) = MathHelper.TwoProd(left.x0, right.x0);
            return MathHelper.QuickTwoSumD(p1,
                p2 + left.x1 * right.x0 + left.x1 * right.x0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DdReal operator *(DdReal left, double right)
        {
            var (p1, p2) = MathHelper.TwoProd(left.x0, right);
            return MathHelper.QuickTwoSumD(p1, p2 + left.x1 * right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DdReal operator *(double left, DdReal right) => right * left;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DdReal Multiply(double a, double b)
        {
            var (p, e) = MathHelper.TwoProd(a, b);
            return new DdReal(p, e);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DdReal Multiply(double other) => this * other;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DdReal Multiply(DdReal other) => this * other;

        #endregion

        #region Division

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DdReal operator /(DdReal left, DdReal right)
        {
            var q1 = left.x0 / right.x0;
            var r = left - q1 / right;

            var q2 = r.x0 / right.x0;
            r -= q2 * right;

            var q3 = r.x0 / right.x0;
            return MathHelper.QuickTwoSumD(q1, q2) + q3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DdReal operator /(DdReal left, double right)
        {
            var a = left.x0;
            var q1 = a / right;

            var (p1, p2) = MathHelper.TwoProd(q1, right);
            var (s, e) = MathHelper.TwoDiff(a, p1);
            e += left.x1;
            e -= p2;

            return MathHelper.QuickTwoSumD(q1, (s + e) / right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DdReal operator /(double left, DdReal right) => (DdReal) left / right;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DdReal Divide(double a, double b)
        {
            var q1 = a / b;

            var (p1, p2) = MathHelper.TwoProd(q1, b);
            var (s, e) = MathHelper.TwoDiff(a, p1);
            e -= p2;

            return MathHelper.QuickTwoSumD(q1, (s + e) / b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DdReal Divide(double other) => this / other;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DdReal Divide(DdReal other) => this / other;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DdReal DivideSloppy(DdReal other) => this / other;

        #endregion

        #region Other math operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsNaN()
        {
            return double.IsNaN(x0) | double.IsNaN(x1);
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

        public static implicit operator DdReal(double value) => new DdReal(value);

        public static implicit operator DdReal(float value) => new DdReal(value);

        public static explicit operator double(DdReal value) => value.x0;

        public static explicit operator decimal(DdReal value)
            => (decimal) value.x0 + (decimal) value.x1;

        public static explicit operator float(DdReal value) => (float) value.x0;

        public static explicit operator ulong(DdReal value) => (ulong) value.x0;

        public static explicit operator long(DdReal value) => (long) value.x0;

        public static explicit operator uint(DdReal value) => (uint) value.x0;

        public static explicit operator int(DdReal value) => (int) value.x0;

        public static explicit operator short(DdReal value) => (short) value.x0;

        public static explicit operator ushort(DdReal value) => (ushort) value.x0;

        public static explicit operator byte(DdReal value) => (byte) value.x0;

        public static explicit operator sbyte(DdReal value) => (sbyte) value.x0;

        public static explicit operator char(DdReal value) => (char) value.x0;

        TypeCode IConvertible.GetTypeCode() => TypeCode.Double;

        bool IConvertible.ToBoolean(IFormatProvider provider)
            => throw new InvalidCastException("Cannot cast DdReal to bool");

        byte IConvertible.ToByte(IFormatProvider provider) => (byte) this;

        char IConvertible.ToChar(IFormatProvider provider) => (char) this;

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
            => throw new InvalidCastException("Cannot cast DdReal to DateTime");

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

        #region Relational operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(DdReal left, DdReal right) => left.Equals(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(DdReal left, DdReal right) => !left.Equals(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(DdReal left, DdReal right) => left.CompareTo(right) < 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(DdReal left, DdReal right) => left.CompareTo(right) > 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(DdReal left, DdReal right) => left.CompareTo(right) <= 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(DdReal left, DdReal right) => left.CompareTo(right) >= 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(DdReal other)
        {
            var cmp = x0.CompareTo(other.x0);
            if (cmp != 0)
            {
                return cmp;
            }

            return x1.CompareTo(other.x1);
        }

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return 1;
            }

            return obj is DdReal other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(DdReal)}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(DdReal other) => x0.Equals(other.x0) && x1.Equals(other.x1);

        public override bool Equals(object obj) => obj is DdReal other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                return (x0.GetHashCode() * 397) ^ x1.GetHashCode();
            }
        }

        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Deconstruct(out double high, out double low)
        {
            high = x0;
            low = x1;
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

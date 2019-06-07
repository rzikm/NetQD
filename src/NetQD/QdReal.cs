using System;
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
    }
}

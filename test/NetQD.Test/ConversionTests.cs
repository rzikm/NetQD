using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using Xunit;
using Xunit.Sdk;

namespace NetQD.Test
{
    public abstract class ConversionData<T> : TheoryData<T, Type, object>
        where T : IConvertible
    {
        private readonly Func<double, T> create;

        protected ConversionData(Func<double, T> create)
        {
            this.create = create;

            MyAdd(5, 5.0);
            MyAdd(5.5, 5.5);
            MyAdd(1, 1L);
            MyAdd(1, 1UL);
            MyAdd(1, 1U);
            MyAdd(1, 1);
            MyAdd(1, 1f);
            MyAdd(1, (decimal) 1);
            MyAdd(1, (short) 1);
            MyAdd(1, (ushort) 1);
            MyAdd(1, (byte) 1);
            MyAdd(1, (sbyte) 1);
            MyAdd(1, (char) 1);
        }

        protected void MyAdd<TTarget>(double value, TTarget expected) => base.Add(create(value), typeof(TTarget), expected);
    }

    public class DdRealConversionData : ConversionData<DdReal>
    {
        public DdRealConversionData()
            : base(v => new DdReal(v))
        {
        }
    }

    public class QdRealConversionData : ConversionData<QdReal>
    {
        public QdRealConversionData() : base(v => new QdReal(v))
        {
            // also check conversions between QdReal and DdReal
            Add(QdReal.One, typeof(DdReal), DdReal.One);
            Add((QdReal) DdReal.Divide(1,7), typeof(DdReal), DdReal.Divide(1,7));
        }
    }

    public abstract class ConversionTests<T, TData> where T : IConvertible where TData : IEnumerable<object[]>, new()
    {
        protected abstract T Create(double src);

        public static IEnumerable<object[]> GetData() => new TData();

        [Theory]
        [MemberData(nameof(GetData))]
        public void SuccessfulConversion(T instance, Type targetType, object expected)
        {
            Assert.Equal(expected, Convert.ChangeType(instance, targetType));
            // other way would throw InvalidCastException because of boxing?
        }

        [Theory]
        [InlineData(5, typeof(bool))]
        [InlineData(5, typeof(DateTime))]
        public void UnsuccessfulConversion(double src, Type targetType)
        {
            // try both ways 
            Assert.Throws<InvalidCastException>(()
                => Convert.ChangeType(Create(src), targetType));

            Assert.Throws<InvalidCastException>(()
                => Convert.ChangeType(Activator.CreateInstance(targetType), typeof(T)));
        }

        [Fact]
        public void GetTypeCode_ReturnsDouble()
        {
            Assert.Equal(TypeCode.Double, Create(0).GetTypeCode());
        }
    }

    public class DdConversionTests : ConversionTests<DdReal, DdRealConversionData>
    {
        protected override DdReal Create(double src) => new DdReal(src);

        [Fact]
        public void ConversionOperatorsFromDdReal()
        {
            double value = 1;

            Assert.Equal((DdReal) (decimal)value, Create(value));
            Assert.Equal((DdReal) (double)value, Create(value));
            Assert.Equal((DdReal) (float)value, Create(value));
            Assert.Equal((DdReal) (ulong)value, Create(value));
            Assert.Equal((DdReal) (long)value, Create(value));
            Assert.Equal((DdReal) (uint)value, Create(value));
            Assert.Equal((DdReal) (int)value, Create(value));
            Assert.Equal((DdReal) (short)value, Create(value));
            Assert.Equal((DdReal) (ushort)value, Create(value));
            Assert.Equal((DdReal) (byte)value, Create(value));
            Assert.Equal((DdReal) (sbyte)value, Create(value));
            Assert.Equal((DdReal) (char)value, Create(value));
        }
    }

    public class QdConversionTests : ConversionTests<QdReal, QdRealConversionData>
    {
        protected override QdReal Create(double src) => new QdReal(src);

        [Fact]
        public void ConversionOperatorsFromQdReal()
        {
            double value = 1;

            Assert.Equal((QdReal) (DdReal)value, Create(value));
            Assert.Equal((QdReal) (decimal)value, Create(value));
            Assert.Equal((QdReal) (double)value, Create(value));
            Assert.Equal((QdReal) (float)value, Create(value));
            Assert.Equal((QdReal) (ulong)value, Create(value));
            Assert.Equal((QdReal) (long)value, Create(value));
            Assert.Equal((QdReal) (uint)value, Create(value));
            Assert.Equal((QdReal) (int)value, Create(value));
            Assert.Equal((QdReal) (short)value, Create(value));
            Assert.Equal((QdReal) (ushort)value, Create(value));
            Assert.Equal((QdReal) (byte)value, Create(value));
            Assert.Equal((QdReal) (sbyte)value, Create(value));
            Assert.Equal((QdReal) (char)value, Create(value));
        }
    }
}

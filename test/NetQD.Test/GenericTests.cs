using System;
using System.Reflection.Metadata.Ecma335;
using Xunit;
using Xunit.Abstractions;

namespace NetQD.Test
{
    public abstract partial class GenericTests<T>
    {
        [Fact]
        public void HasEnhancedPrecision()
        {
            double a = 8;
            double b = 1e-16;

            // out of range
            Assert.Equal(a + b, a);

            var res = Real.Add(a, b) - a;
            // enhanced precision should give us b back
            Assert.Equal(b, res[0]);
        }

        public abstract class Addition
        {
            [Fact]
            public void Simple()
            {
                double a = 5;
                double b = 7;
                var res = Real.Add(a , b);
                Assert.Equal(12, res[0]);
                Assert.Equal(0, res[1]);
            }

            [Fact]
            public void LargeAbsDiff()
            {
                double a = 8;
                double b = 1e-16;

                // out of range
                Assert.Equal(a + b, a);

                var res = Real.Add(a , b);
                Assert.Equal(a, res[0]);

                Assert.NotEqual(0, res[1]);
                Assert.NotEqual(res, new Real(a + b));
            }
        }

        public abstract class Subtraction
        {

            [Fact]
            public void Simple()
            {
                double a = 5;
                double b = 7;
                var res = Real.Subtract(a, b);
                Assert.Equal(-2, res[0]);
                Assert.Equal(0, res[1]);
            }
        }

        public  abstract class Multiplication
        {
            [Fact]
            public void Simple()
            {
                double a = 5;
                double b = 7;
                var res = Real.Multiply(a , b);
                Assert.Equal(35, res[0]);
                Assert.Equal(0, res[1]);
            }
        }

        public abstract class Division
        {

            [Fact]
            public void FractionExact()
            {
                var res = Real.Divide(1 / 2.0, 4);
                Assert.Equal(1 / 8.0, res[0]);

                // fractions of power of 2 can be represented exactly in IEEE format
                Assert.Equal(0, res[1]);
            }

            [Fact]
            public void Fraction()
            {
                var res = Real.Divide(1 / 2.0, 3.0);
                Assert.Equal(1 / 6.0, res[0]);

                // Expecting a remainder 
                Assert.NotEqual(0, res[1]);
            }

            [Fact]
            public void Integral()
            {
                var res = Real.Divide(6, 2);
                Assert.Equal(3, res[0]);
                Assert.Equal(0, res[1]);
            }
        }

    }

    // instantiate test suites

    public class DdRealTests : GenericTests<DdReal>
    {
        public new class Addition : GenericTests<DdReal>.Addition
        {
        }
        public new class Subtraction : GenericTests<DdReal>.Subtraction
        {
        }
        public new class Multiplication : GenericTests<DdReal>.Multiplication
        {
        }
        public new class Division : GenericTests<DdReal>.Division
        {
        }
    }
    public class QdRealTests : GenericTests<QdReal>
    {
        public new class Addition : GenericTests<QdReal>.Addition
        {
        }
        public new class Subtraction : GenericTests<QdReal>.Subtraction
        {
        }
        public new class Multiplication : GenericTests<QdReal>.Multiplication
        {
        }
        public new class Division : GenericTests<QdReal>.Division
        {
        }
    }
}

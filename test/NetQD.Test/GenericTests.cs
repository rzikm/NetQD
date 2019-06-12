using System;
using System.Reflection.Metadata.Ecma335;
using Xunit;
using Xunit.Abstractions;

namespace NetQD.Test
{
    public class RelationalOperatorData : TheoryData<double, double>
    {
        public RelationalOperatorData()
        {
            Add(0, 0);
            Add(1, 0);
            Add(0, 1);
        }
    }

    public class OperatorCompData : TheoryData<double, double>
    {
        public OperatorCompData()
        {
            Add(10, 5);
            Add(-5, 10);
            Add(5, 0);
            Add(1/3.0, 17);
            Add(-23, -17);
            Add(-1/213.0, -1/213.0);
        }
    }

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

        [Theory]
        [ClassData(typeof(RelationalOperatorData))]
        public void ComparisonOperators(double left, double right)
        {
            Real l = new Real(left);
            Real r = new Real(right);

            Assert.Equal(left < right, l < r);
            Assert.Equal(left <= right, l <= r);
            Assert.Equal(left > right, l > r);
            Assert.Equal(left >= right, l >= r);
            Assert.Equal(left == right, l == r);
            Assert.Equal(left != right, l != r);
        }

        public abstract class OperatorsVsMethods
        {
            private void DoTest(double left, double right, 
                Func<double, double, Real> expected,
                Func<Real, Real, Real> opBoth,
                Func<Real, double, Real> opLeft,
                Func<double, Real, Real> opRight,
                Func<Real, Real, Real> methBoth,
                Func<Real, double, Real> methLeft,
                Func<Real, Real, Real> methSloppy)
            {
                var expect = expected(left, right);

                Assert.Equal(expect[0], opBoth(new Real(left), new Real(right))[0]);
                Assert.Equal(expect[0], opLeft(new Real(left), right)[0]);
                Assert.Equal(expect[0], opRight(left, new Real(right))[0]);
                Assert.Equal(expect[0], methBoth(new Real(left), new Real(right))[0]);
                Assert.Equal(expect[0], methLeft(new Real(left), right)[0]);
                Assert.Equal(expect[0], methSloppy(new Real(left), new Real(right))[0]);
            }

            [Theory]
            [ClassData(typeof(OperatorCompData))]
            public void Addition(double left, double right)
            {
                DoTest(left, right,
                    Real.Add,
                    (a, b) => a + b,
                    (a, b) => a + b,
                    (a, b) => a + b,
                    (a, b) => a.Add(b),
                    (a, b) => a.Add(b),
                    (a, b) => a.AddSloppy(b));
            }

            [Theory]
            [ClassData(typeof(OperatorCompData))]
            public void Subtraction(double left, double right)
            {
                DoTest(left, right,
                    Real.Subtract,
                    (a, b) => a - b,
                    (a, b) => a - b,
                    (a, b) => a - b,
                    (a, b) => a.Subtract(b),
                    (a, b) => a.Subtract(b),
                    (a, b) => a.SubtractSloppy(b));
            }

            [Theory]
            [ClassData(typeof(OperatorCompData))]
            public void Multiplication(double left, double right)
            {
                DoTest(left, right,
                    Real.Multiply,
                    (a, b) => a * b,
                    (a, b) => a * b,
                    (a, b) => a * b,
                    (a, b) => a.Multiply(b),
                    (a, b) => a.Multiply(b),
                    (a, b) => a.MultiplySloppy(b));
            }

            [Theory]
            [ClassData(typeof(OperatorCompData))]
            public void Division(double left, double right)
            {
                DoTest(left, right,
                    Real.Divide,
                    (a, b) => a / b,
                    (a, b) => a / b,
                    (a, b) => a / b,
                    (a, b) => a.Divide(b),
                    (a, b) => a.Divide(b),
                    (a, b) => a.DivideSloppy(b));
            }
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
        public class Operators : GenericTests<DdReal>.OperatorsVsMethods
        {
        }
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
        public class Operators : GenericTests<QdReal>.OperatorsVsMethods
        {
        }
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

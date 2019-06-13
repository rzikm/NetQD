using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetQD.Test
{
    partial class GenericTests<T> where T : IComparable, IComparable<T>, IEquatable<T>
    {
        // internal wrapper to be used in generic tests in place of DdReal and QdReal
        internal class Real : IComparable, IComparable<T>, IEquatable<T>
        {
            private static readonly IOperationsProvider<T> op;
            private readonly T value;

            static Real()
            {
                if (typeof(T) == typeof(DdReal))
                {
                    op = (IOperationsProvider<T>)new DdOperationsProvider();
                }
                else if (typeof(T) == typeof(QdReal))
                {
                    op = (IOperationsProvider<T>)new QdOperationsProvider();
                }
                else
                {
                    Debug.Fail("Should not get there");
                }
            }

            public Real(T value) => this.value = value;
            public Real(double value) => this.value = op.Create(value);

            public static Real Parse(string s) => op.Parse(s);
            public static bool TryParse(string s, out Real v)
            {
                var succ = op.TryParse(s, out var vv);
                v = new Real(vv);
                return succ;
            }

            public static implicit operator T(Real inst) => inst.value;

            public static implicit operator Real(T inst) => new Real(inst);

            public double this[int index] => op.Get(this, index);

            public static Real operator +(Real left, Real right)
                => op.AddOperator(left, right);
            public static Real operator +(Real left, double right) => op.AddOperator(left, right);
            public static Real operator +(double left, Real right) => op.AddOperator(left, right);
            public static Real Add(double a, double b) => op.Add(a, b);
            public Real Add(double other) => op.AddMember(this, other);
            public Real Add(Real other) => op.AddMember(this, other);
            public Real AddSloppy(Real other) => op.AddSloppy(this, other);

            public static Real operator -(Real left, Real right)
                => op.SubtractOperator(left, right);
            public static Real operator -(Real left, double right)
                => op.SubtractOperator(left, right);
            public static Real operator -(double left, Real right)
                => op.SubtractOperator(left, right);
            public static Real Subtract(double a, double b) => op.Subtract(a, b);
            public Real Subtract(double other) => op.SubtractMember(this, other);
            public Real Subtract(Real other) => op.SubtractMember(this, other);
            public Real SubtractSloppy(Real other) => op.SubtractSloppy(this, other);


            public static Real operator *(Real left, Real right)
                => op.MultiplyOperator(left, right);
            public static Real operator *(Real left, double right)
                => op.MultiplyOperator(left, right);
            public static Real operator *(double left, Real right)
                => op.MultiplyOperator(left, right);
            public static Real Multiply(double a, double b) => op.Multiply(a, b);
            public Real Multiply(double other) => op.MultiplyMember(this, other);
            public Real Multiply(Real other) => op.MultiplyMember(this, other);
            public Real MultiplySloppy(Real other) => op.MultiplySloppy(this, other);


            public static Real operator /(Real left, Real right)
                => op.DivideOperator(left, right);
            public static Real operator /(Real left, double right)
                => op.DivideOperator(left, right);
            public static Real operator /(double left, Real right)
                => op.DivideOperator(left, right);
            public static Real Divide(double a, double b) => op.Divide(a, b);
            public Real Divide(double other) => op.DivideMember(this, other);
            public Real Divide(Real other) => op.DivideMember(this, other);
            public Real DivideSloppy(Real other) => op.DivideSloppy(this, other);

            public int CompareTo(object obj) => value.CompareTo(obj);
            public int CompareTo(T other) => value.CompareTo(other);
            public bool Equals(T other) => value.Equals(other);

            public static bool operator <(Real left, Real right) => op.Lesser(left, right);
            public static bool operator >(Real left, Real right) => op.Greater(left, right);
            public static bool operator <=(Real left, Real right) => op.LesserEqual(left, right);
            public static bool operator >=(Real left, Real right) => op.GreaterEqual(left, right);
            public static bool operator ==(Real left, Real right) => op.Equal(left, right);
            public static bool operator !=(Real left, Real right) => op.NotEqual(left, right);

            public bool IsNan() => op.IsNaN(this);
            public bool IsFinite() => op.IsFinite(this);
            public bool IsInfinity() => op.IsInfinity(this);
            public bool IsPositiveInfinity() => op.IsPositiveInfinity(this);
            public bool IsNegativeInfinity() => op.IsNegativeInfinity(this);
        }
    }
}

using System.Reflection.Metadata.Ecma335;

namespace NetQD.Test
{
    internal class QdOperationsProvider : IOperationsProvider<QdReal>
    {
        public QdReal Create(double a) => new QdReal(a);
        public QdReal Parse(string s) => QdReal.Parse(s);
        public bool TryParse(string s, out QdReal v) => QdReal.TryParse(s, out v);

        public double Get(QdReal instance, int index) => instance[index];

        public QdReal Add(double a, double b) => QdReal.Add(a, b);
        public QdReal AddMember(QdReal a, QdReal b) => a.Add(b);
        public QdReal AddMember(QdReal a, double b) => a.Add(b);
        public QdReal AddOperator(QdReal a, QdReal b) => a + b;
        public QdReal AddOperator(QdReal a, double b) => a + b;
        public QdReal AddOperator(double a, QdReal b) => a + b;
        public QdReal AddSloppy(QdReal a, QdReal b) => a.AddSloppy(b);

        public QdReal Subtract(double a, double b) => QdReal.Subtract(a, b);
        public QdReal SubtractMember(QdReal a, QdReal b) => a.Subtract(b);
        public QdReal SubtractMember(QdReal a, double b) => a.Subtract(b);
        public QdReal SubtractOperator(QdReal a, QdReal b) => a - b;
        public QdReal SubtractOperator(QdReal a, double b) => a - b;
        public QdReal SubtractOperator(double a, QdReal b) => a - b;
        public QdReal SubtractSloppy(QdReal a, QdReal b) => a.SubtractSloppy(b);

        public QdReal Multiply(double a, double b) => QdReal.Multiply(a, b);
        public QdReal MultiplyMember(QdReal a, QdReal b) => a.Multiply(b);
        public QdReal MultiplyMember(QdReal a, double b) => a.Multiply(b);
        public QdReal MultiplyOperator(QdReal a, QdReal b) => a * b;
        public QdReal MultiplyOperator(QdReal a, double b) => a * b;
        public QdReal MultiplyOperator(double a, QdReal b) => a * b;
        public QdReal MultiplySloppy(QdReal a, QdReal b) => a.MultiplySloppy(b);

        public QdReal Divide(double a, double b) => QdReal.Divide(a, b);
        public QdReal DivideMember(QdReal a, QdReal b) => a.Divide(b);
        public QdReal DivideMember(QdReal a, double b) => a.Divide(b);
        public QdReal DivideOperator(QdReal a, QdReal b) => a / b;
        public QdReal DivideOperator(QdReal a, double b) => a / b;
        public QdReal DivideOperator(double a, QdReal b) => a / b;
        public QdReal DivideSloppy(QdReal a, QdReal b) => a.DivideSloppy(b);

        public bool GreaterEqual(QdReal a, QdReal b) => a >= b;
        public bool Greater(QdReal a, QdReal b) => a > b;
        public bool Lesser(QdReal a, QdReal b) => a < b;
        public bool LesserEqual(QdReal a, QdReal b) => a <= b;
        public bool Equal(QdReal a, QdReal b) => a == b;
        public bool NotEqual(QdReal a, QdReal b) => a != b;

        public bool IsNaN(QdReal a) => a.IsNaN();
        public bool IsFinite(QdReal a) => a.IsFinite();
        public bool IsInfinity(QdReal a) => a.IsInfinity();
        public bool IsPositiveInfinity(QdReal a) => a.IsPositiveInfinity();
        public bool IsNegativeInfinity(QdReal a) => a.IsNegativeInfinity();
    }
}
namespace NetQD.Test
{
    internal class DdOperationsProvider : IOperationsProvider<DdReal>
    {
        public DdReal Create(double a) => new DdReal(a);
        public double Get(DdReal instance, int index) => instance[index];

        public DdReal Add(double a, double b) => DdReal.Add(a, b);
        public DdReal AddMember(DdReal a, DdReal b) => a.Add(b);
        public DdReal AddMember(DdReal a, double b) => a.Add(b);
        public DdReal AddOperator(DdReal a, DdReal b) => a + b;
        public DdReal AddOperator(DdReal a, double b) => a + b;
        public DdReal AddOperator(double a, DdReal b) => a + b;
        public DdReal AddSloppy(DdReal a, DdReal b) => a.AddSloppy(b);

        public DdReal Subtract(double a, double b) => DdReal.Subtract(a, b);
        public DdReal SubtractMember(DdReal a, DdReal b) => a.Subtract(b);
        public DdReal SubtractMember(DdReal a, double b) => a.Subtract(b);
        public DdReal SubtractOperator(DdReal a, DdReal b) => a - b;
        public DdReal SubtractOperator(DdReal a, double b) => a - b;
        public DdReal SubtractOperator(double a, DdReal b) => a - b;
        public DdReal SubtractSloppy(DdReal a, DdReal b) => a.SubtractSloppy(b);

        public DdReal Multiply(double a, double b) => DdReal.Multiply(a, b);
        public DdReal MultiplyMember(DdReal a, DdReal b) => a.Multiply(b);
        public DdReal MultiplyMember(DdReal a, double b) => a.Multiply(b);
        public DdReal MultiplyOperator(DdReal a, DdReal b) => a * b;
        public DdReal MultiplyOperator(DdReal a, double b) => a * b;
        public DdReal MultiplyOperator(double a, DdReal b) => a * b;
        public DdReal MultiplySloppy(DdReal a, DdReal b) => a.MultiplySloppy(b);

        public DdReal Divide(double a, double b) => DdReal.Divide(a, b);
        public DdReal DivideMember(DdReal a, DdReal b) => a.Divide(b);
        public DdReal DivideMember(DdReal a, double b) => a.Divide(b);
        public DdReal DivideOperator(DdReal a, DdReal b) => a / b;
        public DdReal DivideOperator(DdReal a, double b) => a / b;
        public DdReal DivideOperator(double a, DdReal b) => a / b;
        public DdReal DivideSloppy(DdReal a, DdReal b) => a.DivideSloppy(b);
    }
}
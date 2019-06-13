namespace NetQD.Test
{
    internal interface IOperationsProvider<T>
    {
        T Create(double a);
        T Parse(string s);
        bool TryParse(string s, out T v);

        double Get(T instance, int index);

        T Add(double a, double b);
        T AddMember(T a, T b);
        T AddMember(T a, double b);
        T AddOperator(T a, T b);
        T AddOperator(T a, double b);
        T AddOperator(double a, T b);
        T AddSloppy(T a, T b);

        T Subtract(double a, double b);
        T SubtractMember(T a, T b);
        T SubtractMember(T a, double b);
        T SubtractOperator(T a, T b);
        T SubtractOperator(T a, double b);
        T SubtractOperator(double a, T b);
        T SubtractSloppy(T a, T b);

        T Multiply(double a, double b);
        T MultiplyMember(T a, T b);
        T MultiplyMember(T a, double b);
        T MultiplyOperator(T a, T b);
        T MultiplyOperator(T a, double b);
        T MultiplyOperator(double a, T b);
        T MultiplySloppy(T a, T b);

        T Divide(double a, double b);
        T DivideMember(T a, T b);
        T DivideMember(T a, double b);
        T DivideOperator(T a, T b);
        T DivideOperator(T a, double b);
        T DivideOperator(double a, T b);
        T DivideSloppy(T a, T b);

        bool GreaterEqual(T a, T b);
        bool Greater(T a, T b);
        bool Lesser(T a, T b);
        bool LesserEqual(T a, T b);
        bool Equal(T a, T b);
        bool NotEqual(T a, T b);

        bool IsNaN(T a);
        bool IsFinite(T a);
        bool IsInfinity(T a);
        bool IsPositiveInfinity(T a);
        bool IsNegativeInfinity(T a);
    }
}
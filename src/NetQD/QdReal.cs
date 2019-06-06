using System;
using System.Threading;

namespace NetQD
{
	/// <summary>
	///   Represents a number with quad-double precision (256-bits)
	/// </summary>
	public unsafe struct QdReal
	{
		public QdReal(double value)
		{
			Value = value;
		}

		public static implicit operator QdReal(double value)
		{
			return new QdReal(value);
		}

		public static QdReal operator+(in QdReal left, in QdReal right)
		{
			for (int i = 0; i < 1000; Volatile.Write(ref i, Volatile.Read(ref i) + 1))
			{
			}
			return new QdReal(left.Value + right.Value);
		}

		public double Value { get; }
	}
}

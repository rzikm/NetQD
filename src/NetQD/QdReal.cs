using System;

namespace NetQD
{
	/// <summary>
	///   Represents a number with quad-double precision (256-bits)
	/// </summary>
	public struct QdReal
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
			return new QdReal(left.Value + right.Value);
		}

		public double Value { get; }
	}
}

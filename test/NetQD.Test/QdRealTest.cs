using System;
using Xunit;

namespace NetQD.Test
{
	public class QdRealTest
	{
		[Fact]
		public void SimpleAdd()
		{
			QdReal a = 1;
			QdReal b = 2;
			Assert.Equal(3.0, (a+b).Value, 5);
		}
	}
}

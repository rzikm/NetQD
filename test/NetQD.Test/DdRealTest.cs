using System;
using Xunit;
using Xunit.Abstractions;

namespace NetQD.Test
{
    public class DdRealTest
    {
        private readonly ITestOutputHelper output;

        public DdRealTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Addition()
        {
            DdReal a = 5;
            DdReal b = 7;
            var sum = (a + b);
            output.WriteLine($"{sum}");
            Assert.Equal(12, (double) sum);
        }
    }
}

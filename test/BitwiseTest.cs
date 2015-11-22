using Xunit;
using mr.system;

namespace test {
  public class BitwiseTest {

    [Theory]
    [InlineData(0, 0, false)]
    [InlineData(0, 1, false)]
    [InlineData(0, 100, false)]
    [InlineData(1, 0, true)]
    [InlineData(1, 1, false)]
    [InlineData(5, 3, false)]
    [InlineData(5, 2, true)]
    [InlineData(5, 0, true)]
    public void GetBit(int value, int bit, bool output) {
      Assert.Equal(output, Utils.Bit(value, bit));
    }

    [Theory]
    [InlineData(0, 0, false, 0)]
    [InlineData(0, 0, true, 1)]
    [InlineData(0, 1, true, 2)]
    [InlineData(1, 0, false, 0)]
    [InlineData(1, 1, false, 1)]
    [InlineData(9, 0, false, 8)]
    [InlineData(9, 1, true, 11)]
    public void SetBit(byte value, int bit, bool state, byte output) {
      Assert.Equal(output, Utils.Bit(value, bit, state));
    }
  }
}

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

    [Theory]
    [InlineData(0, true)]
    [InlineData(1, true)]
    [InlineData(0x58, true)]
    [InlineData(0x7E, true)]
    [InlineData(0x7F, true)]
    [InlineData(0x80, false)]
    [InlineData(0x81, false)]
    [InlineData(0xA5, false)]
    [InlineData(0xFF, false)]
    public void Sign(byte value, bool sign) {
      Assert.Equal(sign, Utils.Sign(value));
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(0x58, 0x58)]
    [InlineData(0x7E, 0x7E)]
    [InlineData(0x7F, 0x7F)]
    [InlineData(0x80, -128)]
    [InlineData(0x81, -127)]
    [InlineData(0xFE, -2)]
    [InlineData(0xFF, -1)]
    public void Signed(byte unsigned, sbyte signed) {
      Assert.Equal(signed, Utils.Signed(unsigned));
    }
  }
}

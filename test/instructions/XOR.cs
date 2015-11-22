using Xunit;

namespace test.instructions {

  public class XOR : InstructionTests {

    public XOR() {
      opcodes = new byte[] {
        0xA8, 0xA9, 0xAA, 0xAB, 0xAC, 0xAD, 0xAE, 0xEE
      };
    }

    void Test(byte left, byte right, Step assert) {
      AllOpcodes(
        (operands) => {
          cpu.registers.a = left;
          operands[0].Target = right;
        },
        assert);
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(0x0F, 0xF0, 0xFF)]
    [InlineData(0xFF, 0xFF, 0)]
    [InlineData(0x0F, 0xFF, 0xF0)]
    public void ShouldLogicalXor(byte left, byte right, byte output) {
      Test(left, right, operands => Assert.Equal(output, cpu.registers.a));
    }

    [Theory]
    [InlineData(0, 0, false)]
    [InlineData(0x0F, 0xF0, true)]
    [InlineData(0xFF, 0xFF, false)]
    [InlineData(0xA4, 0xFF, false)]
    [InlineData(0x99, 0, true)]
    public void ShouldHandleSignFlag(byte left, byte right, bool sign) {
      Test(left, right, operands => Assert.Equal(sign, cpu.Sign));
    }

    [Theory]
    [InlineData(0, 0, true)]
    [InlineData(0x0F, 0xF0, false)]
    [InlineData(0xFF, 0xFF, true)]
    [InlineData(0xFF, 1, false)]
    public void ShouldHandleZeroFlag(byte left, byte right, bool zero) {
      Test(left, right, operands => Assert.Equal(zero, cpu.Zero));
    }

    [Theory]
    [InlineData(0, 0, true)]
    [InlineData(0x0F, 0xF0, true)]
    [InlineData(0xFF, 0xFF, true)]
    [InlineData(0xFF, 1, false)]
    [InlineData(0xA4, 0, false)]
    public void ShouldHandleParityFlag(byte left, byte right, bool parity) {
      Test(left, right, operands => Assert.Equal(parity, cpu.Overflow));
    }

  }

  // Special tests for "xor a"
  public class XOR_A : InstructionTests {
  
    [Theory]
    [InlineData(0)]
    [InlineData(0x0F)]
    [InlineData(0x3A)]
    [InlineData(0xFF)]
    public void ShouldLogicalOr(byte input) {
      cpu.registers.a = input;
      cpu.Apply(0xAF).Do();
      Assert.Equal(0, cpu.registers.a);
    }
    
  }

}

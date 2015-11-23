using Xunit;

namespace test.instructions {

  public class XOR : InstructionTests {

    public XOR() {
      opcodes = new byte[] {
        0xA8, 0xA9, 0xAA, 0xAB, 0xAC, 0xAD, 0xAE, 0xEE
      };
    }

    void Test(byte left, byte right, Step setup, Step assert) {
      AllOpcodes(
        operands => {
          cpu.registers.a = left;
          operands[0].Target = right;
          setup.Invoke(operands);
        },
        assert);
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(0x0F, 0xF0, 0xFF)]
    [InlineData(0xFF, 0xFF, 0)]
    [InlineData(0x0F, 0xFF, 0xF0)]
    public void ShouldLogicalXor(byte left, byte right, byte output) {
      Test(left, right, operands => { }, operands => Assert.Equal(output, cpu.registers.a));
    }

    [Theory]
    [InlineData(0, 0, true, false)]
    [InlineData(0x0F, 0xF0, false, true)]
    [InlineData(0x99, 0, false, true)]
    [InlineData(0x99, 0, true, true)]
    [InlineData(0xA4, 0xFF, true, false)]
    [InlineData(0xFF, 0xFF, false, false)]
    public void ShouldHandleSignFlag(byte left, byte right, bool signInput, bool signOutput) {
      Test(left, right, operands => cpu.Sign = signInput, operands => Assert.Equal(signOutput, cpu.Sign));
    }

    [Theory]
    [InlineData(0, 0, false, true)]
    [InlineData(0x0F, 0xF0, false, false)]
    [InlineData(0xFF, 0xFF, true, true)]
    [InlineData(0xFF, 1, true, false)]
    public void ShouldHandleZeroFlag(byte left, byte right, bool zeroInput, bool zeroOutput) {
      Test(left, right, operands => cpu.Zero = zeroInput, operands => Assert.Equal(zeroOutput, cpu.Zero));
    }

    [Theory]
    [InlineData(0, 0, false, true)]
    [InlineData(0x0F, 0xF0, true, true)]
    [InlineData(0xFF, 0xFF, false, true)]
    [InlineData(0xFF, 1, false, false)]
    [InlineData(0xA4, 0, true, false)]
    public void ShouldHandleParityFlag(byte left, byte right, bool parityInput, bool parityOutput) {
      Test(left, right, operands => cpu.Overflow = parityInput, operands => Assert.Equal(parityOutput, cpu.Overflow));
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

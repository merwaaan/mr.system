using Xunit;

namespace test.instructions {

  public class OR : InstructionTests {

    public OR() {
      opcodes = new byte[] {
        0xB0, 0xB1, 0xB2, 0xB3, 0xB4, 0xB5, 0xB6, 0xF6
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
    [InlineData(0xFF, 0xFF, 0xFF)]
    [InlineData(0xA4, 0xFF, 0xFF)]
    [InlineData(0x0F, 0x10, 0x1F)]
    public void ShouldLogicalOr(byte left, byte right, byte output) {
      Test(left, right, operands => { }, operands => Assert.Equal(output, cpu.registers.a));
    }

    [Theory]
    [InlineData(0, 0, true, false)]
    [InlineData(0x0F, 0xF0, false, true)]
    [InlineData(0xFF, 0xFF, false, true)]
    [InlineData(0xA4, 0xFF, true, true)]
    [InlineData(0, 0x07, false, false)]
    [InlineData(0, 0x07, true, false)]
    public void ShouldHandleSignFlag(byte left, byte right, bool signInput, bool signOutput) {
      Test(left, right, operands => cpu.Sign = signInput, operands => Assert.Equal(signOutput, cpu.Sign));
    }

    [Theory]
    [InlineData(0, 0, false, true)]
    [InlineData(0, 0, true, true)]
    [InlineData(0x0F, 0xF0, false, false)]
    [InlineData(0xFF, 0xFF, true, false)]
    [InlineData(0xFF, 1, true, false)]
    public void ShouldHandleZeroFlag(byte left, byte right, bool zeroInput, bool zeroOutput) {
      Test(left, right, operands => cpu.Zero = zeroInput, operands => Assert.Equal(zeroOutput, cpu.Zero));
    }

    [Theory]
    [InlineData(0, 0, false, true)]
    [InlineData(0x0F, 0xF0, true, true)]
    [InlineData(0xA4, 0, true, false)]
    [InlineData(0xFF, 0xFF, false, true)]
    [InlineData(0xFF, 1, false, true)]
    [InlineData(0, 0x4F, true, false)]
    public void ShouldHandleParityFlag(byte left, byte right, bool parityInput, bool parityOutput) {
      Test(left, right, operands => cpu.Overflow = parityInput, operands => Assert.Equal(parityOutput, cpu.Overflow));
    }

  }

  // Special tests for "or a"
  public class OR_A : InstructionTests {
  
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(0x0F)]
    [InlineData(0x3A)]
    [InlineData(0xFF)]
    public void ShouldLogicalOr(byte input) {
      cpu.registers.a = input;
      cpu.Apply(0xB7).Do();
      Assert.Equal(input, cpu.registers.a);
    }
    
  }

}

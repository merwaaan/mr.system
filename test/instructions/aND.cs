using Xunit;

namespace test.instructions {

  public class AND : InstructionTests {

    public AND() {
      opcodes = new byte[] {
        0xA0, 0xA1, 0xA2, 0xA3, 0xA4, 0xA5, 0xA6, 0xE6
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
    [InlineData(0x0F, 0xF0, 0)]
    [InlineData(0x0F, 0x0F, 0x0F)]
    [InlineData(0xA4, 0xFF, 0xA4)]
    [InlineData(0xBD, 0x72, 0x30)]
    [InlineData(0xFF, 0xFF, 0xFF)]
    public void ShouldLogicalAnd(byte left, byte right, byte output) {
      Test(left, right, operands => {}, operands => Assert.Equal(output, cpu.registers.a));
    }

    [Theory]
    [InlineData(0, 0, false, false)]
    [InlineData(0, 0, true, false)]
    [InlineData(0x0F, 0xF0, true, false)]
    [InlineData(0xFF, 0xFF, false, true)]
    [InlineData(0xA4, 0xFF, true, true)]
    [InlineData(0xBD, 0x72, true, false)]
    public void ShouldHandleSignFlag(byte left, byte right, bool signInput, bool signOutput) {
      Test(left, right, operands => cpu.Sign = signInput, operands => Assert.Equal(signOutput, cpu.Sign));
    }

    [Theory]
    [InlineData(0, 0, false, true)]
    [InlineData(0, 0, true, true)]
    [InlineData(0x0F, 0xF0, false, true)]
    [InlineData(0xFF, 0xFF, false, false)]
    [InlineData(0xFF, 1, true, false)]
    [InlineData(0xA4, 0, true, true)]
    public void ShouldHandleZeroFlag(byte left, byte right, bool zeroInput, bool zeroOutput) {
      Test(left, right, operands => cpu.Sign = zeroInput, operands => Assert.Equal(zeroOutput, cpu.Zero));
    }

    [Theory]
    [InlineData(0, 0, false, true)]
    [InlineData(0x0F, 0xF0, true, true)]
    [InlineData(0xFF, 0xFF, false, true)]
    [InlineData(0xFF, 1, true, false)]
    [InlineData(0xA4, 0, false, true)]
    [InlineData(0x14, 0x85, false, false)]
    public void ShouldHandleParityFlag(byte left, byte right, bool parityInput, bool parityOutput) {
      Test(left, right, operands => cpu.Sign = parityInput, operands => Assert.Equal(parityOutput, cpu.Overflow));
    }

  }

  // Special tests for "and a"
  public class AND_A : InstructionTests {

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(0x0F)]
    [InlineData(0x3A)]
    [InlineData(0xFF)]
    public void ShouldLogicalAnd(byte input) {
      cpu.registers.a = input;
      cpu.Apply(0xA7).Do();
      Assert.Equal(input, cpu.registers.a);
    }

  }
}

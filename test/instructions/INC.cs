using System;
using Xunit;

namespace test.instructions {

  public class INC8 : InstructionTests {

    public INC8() {
      opcodes = new byte[] {
        0x04, 0x0C, 0x14, 0x1C, 0x24, 0x2C, 0x34, 0x3C
      };
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(0x09, 0x0A)]
    [InlineData(0x7F, 0x80)]
    [InlineData(0xFF, 0)]
    public void ShouldIncrement(byte input, byte output) {
      AllOpcodes(
        operands => operands[0].Target = input,
        operands => Assert.Equal(output, operands[0].Target));
    }

    [Theory]
    [InlineData(0, false, false)]
    [InlineData(0, true, false)]
    [InlineData(0x7e, true, false)]
    [InlineData(0x7f, true, true)]
    [InlineData(0x80, false, true)]
    [InlineData(0xFE, false, true)]
    [InlineData(0xFF, false, false)]
    public void ShouldHandleSignFlag(byte input, bool signInput, bool signOutput) {
      AllOpcodes(
        operands => {
          operands[0].Target = input;
          cpu.Sign = signInput;
        },
        operands => Assert.Equal(signOutput, cpu.Sign));
    }

    [Theory]
    [InlineData(0, true, false)]
    [InlineData(0, false, false)]
    [InlineData(1, true, false)]
    [InlineData(0x85, false, false)]
    [InlineData(0xFE, true, false)]
    [InlineData(0xFF, false, true)]
    [InlineData(0xFF, true, true)]
    public void ShouldHandleZeroFlag(byte input, bool zeroInput, bool zeroOutput) {
      AllOpcodes(
        operands => {
          operands[0].Target = input;
          cpu.Zero = zeroInput;
        },
        operands => Assert.Equal(zeroOutput, cpu.Zero));
    }

    [Theory]
    [InlineData(0, true, false)]
    [InlineData(0x2E, false, false)]
    [InlineData(0x2E, true, false)]
    [InlineData(0x2F, false, true)]
    [InlineData(0x8C, true, false)]
    [InlineData(0xFE, false, false)]
    [InlineData(0xFF, false, true)]
    [InlineData(0xFF, true, true)]
    public void ShouldHandleHalfCarryFlag(byte input, bool hcarryInput, bool hcarryOutput) {
      AllOpcodes(
        operands => {
          operands[0].Target = input;
          cpu.HalfCarry = hcarryInput;
        },
        operands => Assert.Equal(hcarryOutput, cpu.HalfCarry));
    }

    [Theory]
    [InlineData(0, false, false)]
    [InlineData(0x1C, true, false)]
    [InlineData(0x7E, false, false)]
    [InlineData(0x7E, true, false)]
    [InlineData(0x7F, false, true)]
    [InlineData(0x7F, true, true)]
    [InlineData(0x80, true, false)]
    [InlineData(0xFF, false, false)]
    public void ShouldHandleOverflowFlag(byte input, bool overflowInput, bool overflowOutput) {
      AllOpcodes(
        operands => {
          operands[0].Target = input;
          cpu.Overflow = overflowInput;
        },
        operands => Assert.Equal(overflowOutput, cpu.Overflow));
    }

  }

  public class INC16 : InstructionTests {

    public INC16() {
      opcodes = new byte[] {
        0x03, 0x13, 0x23, 0x33
      };
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(0x09, 0x0A)]
    [InlineData(0x7F, 0x80)]
    [InlineData(0xFF, 0x100)]
    [InlineData(0xA456, 0xA457)]
    [InlineData(0xFFFF, 0)]
    public void ShouldIncrement(ushort input, ushort output) {
      AllOpcodes(
        operands => operands[0].Target = input,
        operands => Assert.Equal(output, operands[0].Target));
    }

    [Theory]
    [InlineData(0, "10010000")]
    [InlineData(1, "11111111")]
    [InlineData(0x4A, "00000000")]
    [InlineData(0xF0, "10110010")]
    [InlineData(0xFF, "10010111")]
    public void ShouldNotAffectFlags(ushort input, string flags) {
      byte f = Convert.ToByte(flags, 2);

      AllOpcodes(
        operands => {
          operands[0].Target = input;
          cpu.registers.f = f;
        },
        operands => Assert.Equal(f, cpu.registers.f));
    }
    
  }

}

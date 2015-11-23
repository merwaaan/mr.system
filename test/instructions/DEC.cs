using System;
using Xunit;
using mr.system;

namespace test.instructions {

  public class DEC8 : InstructionTests {

    public DEC8() {
      opcodes = new byte[] {
        0x05, 0x0D, 0x15, 0x1D, 0x25, 0x2D, 0x35, 0x3D
      };
    }

    [Theory]
    [InlineData(0, 0xFF)]
    [InlineData(0x09, 0x08)]
    [InlineData(0x7F, 0x7E)]
    [InlineData(0x80, 0x7F)]
    [InlineData(0xFF, 0xFE)]
    public void ShouldDecrement(byte input, byte output) {
      AllOpcodes(
        operands => operands[0].Target = input,
        operands => Assert.Equal(output, operands[0].Target));
    }

    [Theory]
    [InlineData(0, false, true)]
    [InlineData(0, true, true)]
    [InlineData(0x7e, true, false)]
    [InlineData(0x7f, false, false)]
    [InlineData(0x80, true, false)]
    [InlineData(0x81, true, true)]
    [InlineData(0xFE, false, true)]
    [InlineData(0xFF, true, true)]
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
    [InlineData(1, false, true)]
    [InlineData(1, true, true)]
    [InlineData(0x98, true, false)]
    [InlineData(0xFE, false, false)]
    [InlineData(0xFF, true, false)]
    public void ShouldHandleZeroFlag(byte input, bool zeroInput, bool zeroOutput) {
      AllOpcodes(
        operands => {
          operands[0].Target = input;
          cpu.Zero = zeroInput;
        },
        operands => Assert.Equal(zeroOutput, cpu.Zero));
    }

    [Theory]
    [InlineData(0, false, true)]
    [InlineData(0x2E, true, false)]
    [InlineData(0x2F, true, false)]
    [InlineData(0x30, false, true)]
    [InlineData(0x80, true, true)]
    [InlineData(0xFE, false, false)]
    [InlineData(0xFF, true, false)]
    public void ShouldHandleHalfCarryFlag(byte input, bool hcarryInput, bool hcarryOutput) {
      AllOpcodes(
        operands => {
          operands[0].Target = input;
          cpu.HalfCarry = hcarryInput;
        },
        operands => Assert.Equal(hcarryOutput, cpu.HalfCarry));
    }

    [Theory]
    [InlineData(0, true, false)]
    [InlineData(0x1C, false, false)]
    [InlineData(0x7E, false, false)]
    [InlineData(0x7F, true, false)]
    [InlineData(0x80, false, true)]
    [InlineData(0x80, true, true)]
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

  public class DEC16 : InstructionTests {

    public DEC16() {
      opcodes = new byte[] {
        0x0B, 0x1B, 0x2B, 0x3B
      };
    }

    [Theory]
    [InlineData(0, 0xFFFF)]
    [InlineData(0x09, 0x08)]
    [InlineData(0x7F, 0x7E)]
    [InlineData(0xFF, 0xFE)]
    [InlineData(0xA456, 0xA455)]
    [InlineData(0xFFFF, 0xFFFE)]
    public void ShouldDecrement(ushort input, ushort output) {
      AllOpcodes(
        operands => operands[0].Target = input,
        operands => Assert.Equal(output, operands[0].Target));
    }

    [Theory]
    [InlineData(0, "00000000")]
    [InlineData(0x09, "11111111")]
    [InlineData(0x7F, "11110000")]
    [InlineData(0xFF, "10100110")]
    [InlineData(0xA456, "00111110")]
    [InlineData(0xFFFF, "01001001")]
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

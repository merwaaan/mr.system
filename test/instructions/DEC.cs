using System;
using Xunit;
using mr.system;

namespace test.instructions {

  public class DEC8 : InstructionTests {

    public DEC8() {
      opcodes = new byte[] {
        0x05,
        0x0D,
        0x15,
        0x1D,
        0x25,
        0x2D,
        0x35,
        0x3D
      };
    }

    [Theory]
    [InlineData(0, 0xFF)]
    [InlineData(0x09, 0x08)]
    [InlineData(0x7F, 0x7E)]
    [InlineData(0x80, 0x7F)]
    [InlineData(0xFF, 0xFE)]
    public void ShouldDecrement(byte input, byte output) {
      AllOpcodes(operands => operands[0].Target = input,
                 operands => Assert.Equal(output, operands[0].Target));
    }

    [Theory]
    [InlineData(0, true)]
    [InlineData(0x7e, false)]
    [InlineData(0x7f, false)]
    [InlineData(0x80, false)]
    [InlineData(0x81, true)]
    [InlineData(0xFE, true)]
    [InlineData(0xFF, true)]
    public void ShouldHandleSignFlag(byte input, bool sign) {
      AllOpcodes(operands => operands[0].Target = input,
                 operands => Assert.Equal(sign, cpu.Sign));
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(1, true)]
    [InlineData(0x98, false)]
    [InlineData(0xFE, false)]
    [InlineData(0xFF, false)]
    public void ShouldHandleZeroFlag(byte input, bool zero) {
      AllOpcodes(operands => operands[0].Target = input,
                 operands => Assert.Equal(zero, cpu.Zero));
    }

    [Theory]
    [InlineData(0, true)]
    [InlineData(0x2E, false)]
    [InlineData(0x2F, false)]
    [InlineData(0x30, true)]
    [InlineData(0x80, true)]
    [InlineData(0xFE, false)]
    [InlineData(0xFF, false)]
    public void ShouldHandleHalfCarryFlag(byte input, bool halfcarry) {
      AllOpcodes(operands => operands[0].Target = input,
                 operands => Assert.Equal(halfcarry, cpu.HalfCarry));
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(0x1C, false)]
    [InlineData(0x7E, false)]
    [InlineData(0x7F, false)]
    [InlineData(0x80, true)]
    [InlineData(0xFF, false)]
    public void ShouldHandleOverflowFlag(byte input, bool overflow) {
      AllOpcodes(operands => operands[0].Target = input,
                 operands => Assert.Equal(overflow, cpu.Overflow));
    }

  }

  public class DEC16 : InstructionTests {

    public DEC16() {
      opcodes = new byte[] {
        0x0B,
        0x1B,
        0x2B,
        0x3B
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
      AllOpcodes(operands => operands[0].Target = input,
                 operands => Assert.Equal(output, operands[0].Target));
    }
    
  }

}

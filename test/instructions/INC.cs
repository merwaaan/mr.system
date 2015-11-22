using System;
using Xunit;
using mr.system;

namespace test.instructions {

  public class INC8 : InstructionTests {

    public INC8() {
      opcodes = new byte[] {
        0x04,
        0x0C,
        0x14,
        0x1C,
        0x24,
        0x2C,
        0x34,
        0x3C
      };
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(0x09, 0x0A)]
    [InlineData(0x7F, 0x80)]
    [InlineData(0xFF, 0)]
    public void ShouldIncrement(byte input, byte output) {
      AllOpcodes(operands => operands[0].Target = input,
                 operands => Assert.Equal(output, operands[0].Target));
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(0x7e, false)]
    [InlineData(0x7f, true)]
    [InlineData(0x80, true)]
    [InlineData(0xFE, true)]
    [InlineData(0xFF, false)]
    public void ShouldHandleSignFlag(byte input, bool sign) {
      AllOpcodes(operands => operands[0].Target = input,
                 operands => Assert.Equal(sign, cpu.Sign));
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(0x98, false)]
    [InlineData(0xFE, false)]
    [InlineData(0xFF, true)]
    public void ShouldHandleZeroFlag(byte input, bool zero) {
      AllOpcodes(operands => operands[0].Target = input,
                 operands => Assert.Equal(zero, cpu.Zero));
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(0x2E, false)]
    [InlineData(0x2F, true)]
    [InlineData(0x80, false)]
    [InlineData(0xFE, false)]
    [InlineData(0xFF, true)]
    public void ShouldHandleHalfCarryFlag(byte input, bool halfcarry) {
      AllOpcodes(operands => operands[0].Target = input,
                 operands => Assert.Equal(halfcarry, cpu.HalfCarry));
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(0x1C, false)]
    [InlineData(0x7E, false)]
    [InlineData(0x7F, true)]
    [InlineData(0x80, false)]
    [InlineData(0xFF, false)]
    public void ShouldHandleOverflowFlag(byte input, bool overflow) {
      AllOpcodes(operands => operands[0].Target = input,
                 operands => Assert.Equal(overflow, cpu.Overflow));
    }

  }

  public class INC16 : InstructionTests {

    public INC16() {
      opcodes = new byte[] {
        0x03,
        0x13,
        0x23,
        0x33
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
      AllOpcodes(operands => operands[0].Target = input,
                 operands => Assert.Equal(output, operands[0].Target));
    }
    
  }

}

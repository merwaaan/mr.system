using System;
using Xunit;
using mr.system;

namespace test.instructions {

  public class ADD8 : InstructionTests {

    public ADD8() {
      opcodes = new byte[] {
        0x80,
        0x81,
        0x82,
        0x83,
        0x84,
        0x85,
        0x86,
        0x87,
        0xC6
      };
    }

    Step Setup(byte left, byte right) {
      return (operands) => {
        operands[0].Target = left;
        operands[1].Target = right;
      };
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(0, 1, 1)]
    [InlineData(1, 0, 1)]
    [InlineData(0xAE, 0x11, 0xBF)]
    [InlineData(0x15, 0xC0, 0xD5)]
    [InlineData(0x10, 0xFF, 0x09)]
    [InlineData(0xFF, 0xFF, 0xFE)]
    public void ShouldAdd(byte left, byte right, byte output) {
      AllOpcodes(
        Setup(left, right),
        operands => Assert.Equal(output, operands[0].Target));
    }

    [Theory]
    [InlineData(0, 0, false)]
    [InlineData(1, 0x7e, false)]
    [InlineData(0x7f, 1, true)]
    [InlineData(0x80, 0x20, true)]
    [InlineData(0xFE, 0x34, false)]
    public void ShouldHandleSignFlag(byte left, byte right, bool sign) {
      AllOpcodes(
        Setup(left, right),
        operands => Assert.Equal(sign, cpu.Sign));
    }

    [Theory]
    [InlineData(0, 0, true)]
    [InlineData(0, 1, false)]
    [InlineData(0x4D, 0x34, false)]
    [InlineData(0xFE, 1, false)]
    [InlineData(0xFF, 1, true)]
    [InlineData(0xFF, 0xED, false)]
    public void ShouldHandleZeroFlag(byte left, byte right, bool zero) {
      AllOpcodes(
        Setup(left, right),
        operands => Assert.Equal(zero, cpu.Zero));
    }

    [Theory]
    [InlineData(0, 0, false)]
    [InlineData(0, 5, false)]
    [InlineData(0, 0x10, true)]
    [InlineData(0x08, 0x0F, true)]
    [InlineData(0x3C, 7, true)]
    [InlineData(0xFE, 1, false)]
    [InlineData(0xFF, 1, true)]
    public void ShouldHandleHalfCarryFlag(byte left, byte right, bool halfcarry) {
      AllOpcodes(
        Setup(left, right),
        operands => Assert.Equal(halfcarry, cpu.HalfCarry));
    }

    [Theory]
    [InlineData(0, 0, false)]
    [InlineData(0, 0x1C, false)]
    [InlineData(0x7E, 1, false)]
    [InlineData(0x7E, 2, true)]
    [InlineData(0xF0, 0x34, true)]
    [InlineData(0xFF, 1, true)]
    public void ShouldHandleOverflowFlag(byte left, byte right, bool overflow) {
      AllOpcodes(
        Setup(left, right),
        operands => Assert.Equal(overflow, cpu.Overflow));
    }

    [Theory]
    [InlineData(0, 0, false)]
    [InlineData(0, 1, false)]
    [InlineData(0, 0xFF, false)]
    [InlineData(0xFF, 1, true)]
    [InlineData(0xF0, 0x34, true)]
    [InlineData(0xFF, 0, false)]
    public void ShouldHandleCarryFlag(byte left, byte right, bool carry) {
      AllOpcodes(
        Setup(left, right),
        operands => Assert.Equal(carry, cpu.Carry));
    }

  }

  public class ADD16 : InstructionTests {

    public ADD16() {
      opcodes = new byte[] {
        0x09,
        0x19,
        0x29,
        0x39
      };
    }
    
    Step Setup(ushort left, ushort right) {
      return (operands) => {
        operands[0].Target = left;
        operands[1].Target = right;
      };
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(0, 1, 1)]
    [InlineData(1, 0, 1)]
    [InlineData(0xAE, 0x11, 0xBF)]
    [InlineData(0x15, 0xC0, 0xD5)]
    [InlineData(0x10, 0xFF, 0x109)]
    [InlineData(0xFF, 0xFF, 0x01FE)]
    [InlineData(0x5432, 0x1000, 0x6432)]
    [InlineData(0xFFFF, 1, 0)]
    [InlineData(0xFFFF, 0xFFFF, 0xFFFE)]
    public void ShouldAdd(ushort left, ushort right, byte output) {
      AllOpcodes(
        Setup(left, right),
        operands => Assert.Equal(output, operands[0].Target));
    }

    [Theory] // TODO
    [InlineData(0, 0, false)]
    [InlineData(0, 5, false)]
    [InlineData(0, 0x10, true)]
    [InlineData(0x08, 0x0F, true)]
    [InlineData(0x3C, 7, true)]
    [InlineData(0xFE, 1, false)]
    [InlineData(0xFF, 1, true)]
    public void ShouldHandleHalfCarryFlag(ushort left, ushort right, bool halfcarry) {
      AllOpcodes(
        Setup(left, right),
        operands => Assert.Equal(halfcarry, cpu.HalfCarry));
    }

    [Theory]
    [InlineData(0, 0, false)]
    [InlineData(0, 1, false)]
    [InlineData(0, 0xFF, false)]
    [InlineData(0xFF, 1, false)]
    [InlineData(0xF0, 0x34, false)]
    [InlineData(0xFFFF, 1, true)]
    [InlineData(0xE000, 0x5000, true)]
    public void ShouldHandleCarryFlag(ushort left, ushort right, bool carry) {
      AllOpcodes(
        Setup(left, right),
        operands => Assert.Equal(carry, cpu.Carry));
    }

  }

}

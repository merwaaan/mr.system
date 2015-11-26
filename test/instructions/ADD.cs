using System;
using Xunit;
using mr.system;

namespace test.instructions {

  public class ADD_A : InstructionTests {

    public ADD_A() {
      opcodes = new byte[] { 0x80, 0x81, 0x82, 0x83, 0x84, 0x85, 0x86, 0xC6 };
    }

    void Test(byte left, byte right, Step setup, Step assert) {
      AllOpcodes(
        operands => {
          operands[0].Target = left;
          operands[1].Target = right;
          setup.Invoke(operands);
        },
        assert);
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(0, 1, 1)]
    [InlineData(1, 0, 1)]
    [InlineData(0xAE, 0x11, 0xBF)]
    [InlineData(0x15, 0xC0, 0xD5)]
    [InlineData(0x10, 0xFF, 0x0F)]
    [InlineData(0xFF, 0xFF, 0xFE)]
    public void ShouldAdd(byte left, byte right, byte output) {
      Test(left, right, operands => { }, operands => Assert.Equal(output, operands[0].Target));
    }

    [Theory]
    [InlineData(0, 0, true, false)]
    [InlineData(1, 0x7e, true, false)]
    [InlineData(0x7f, 1, false, true)]
    [InlineData(0x80, 0x20, true, true)]
    [InlineData(0xFE, 0x34, true, false)]
    public void ShouldHandleSignFlag(byte left, byte right, bool signInput, bool signOutput) {
      Test(left, right, operands => cpu.Sign = signInput, operands => Assert.Equal(signOutput, cpu.Sign));
    }

    [Theory]
    [InlineData(0, 0, false, true)]
    [InlineData(0, 1, false, false)]
    [InlineData(0x4D, 0x34, true, false)]
    [InlineData(0xFE, 1, true, false)]
    [InlineData(0xFF, 1, false, true)]
    [InlineData(0xFF, 0xED, true, false)]
    public void ShouldHandleZeroFlag(byte left, byte right, bool zeroInput, bool zeroOutput) {
      Test(left, right, operands => cpu.Zero = zeroInput, operands => Assert.Equal(zeroOutput, cpu.Zero));
    }

    [Theory]
    [InlineData(0, 0, true, false)]
    [InlineData(0, 5, false, false)]
    [InlineData(0, 0x10, true, false)]
    [InlineData(0x08, 0x0F, true, true)]
    [InlineData(0x3C, 7, false, true)]
    [InlineData(0xFE, 1, false, false)]
    [InlineData(0xFF, 1, false, true)]
    public void ShouldHandleHalfCarryFlag(byte left, byte right, bool hcarryInput, bool hcarryOutput) {
      Test(left, right, operands => cpu.HalfCarry = hcarryInput, operands => Assert.Equal(hcarryOutput, cpu.HalfCarry));
    }

    [Theory]
    [InlineData(0, 0, true, false)]
    [InlineData(0, 0x1C, false, false)]
    [InlineData(0x7E, 1, false, false)]
    [InlineData(0x7E, 2, false, true)]
    [InlineData(0x50, 0x70, false, true)]
    [InlineData(0xF0, 0x34, true, false)]
    [InlineData(0xFF, 1, false, false)]
    public void ShouldHandleOverflowFlag(byte left, byte right, bool overflowInput, bool overflowOutput) {
      Test(left, right, operands => cpu.Overflow = overflowInput, operands => Assert.Equal(overflowOutput, cpu.Overflow));
    }

    [Theory]
    [InlineData(0, 0, true, false)]
    [InlineData(0, 1, false, false)]
    [InlineData(0, 0xFF, true, false)]
    [InlineData(0xFF, 1, false, true)]
    [InlineData(0xF0, 0x34, false, true)]
    [InlineData(0xFF, 0, true, false)]
    public void ShouldHandleCarryFlag(byte left, byte right, bool carryInput, bool carryOutput) {
      Test(left, right, operands => cpu.Carry = carryInput, operands => Assert.Equal(carryOutput, cpu.Carry));
    }

  }

  public class ADD_HL : InstructionTests {

    public ADD_HL() {
      opcodes = new byte[] { 0x09, 0x19, 0x39 };
    }
    
    void Test(ushort left, ushort right, Step setup, Step assert) {
      AllOpcodes(
        operands => {
          operands[0].Target = left;
          operands[1].Target = right;
          setup.Invoke(operands);
        },
        assert);
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(0, 1, 1)]
    [InlineData(1, 0, 1)]
    [InlineData(0xAE, 0x11, 0xBF)]
    [InlineData(0x15, 0xC0, 0xD5)]
    [InlineData(0x10, 0xFF, 0x10F)]
    [InlineData(0xFF, 0xFF, 0x01FE)]
    [InlineData(0x5432, 0x1000, 0x6432)]
    [InlineData(0xFFFF, 1, 0)]
    [InlineData(0xFFFF, 0xFFFF, 0xFFFE)]
    public void ShouldAdd(ushort left, ushort right, ushort output) {
      Test(left, right, operands => { }, operands => Assert.Equal(output, operands[0].Target));
    }

    [Theory]
    [InlineData(0, 0, false, false)]
    [InlineData(0, 0, true, false)]
    [InlineData(0x800, 0, true, false)]
    [InlineData(0xFFF, 1, false, true)]
    [InlineData(0xFFF, 1, true, true)]
    [InlineData(0xFFF, 0xFFF, false, true)]
    [InlineData(0xFFF, 0xF000, false, false)]
    public void ShouldHandleHalfCarryFlag(ushort left, ushort right, bool hcarryInput, bool hcarryOutput) {
      Test(left, right, operands => cpu.HalfCarry = hcarryInput, operands => Assert.Equal(hcarryOutput, cpu.HalfCarry));
    }

    [Theory]
    [InlineData(0, 0, false, false)]
    [InlineData(0, 0, true, false)]
    [InlineData(0, 1, true, false)]
    [InlineData(0, 0xFF, true, false)]
    [InlineData(0xFF, 1, false, false)]
    [InlineData(0xF0, 0x34, true, false)]
    [InlineData(0xF0, 0x34, false, false)]
    [InlineData(0xFFFF, 1, false, true)]
    [InlineData(0xFFFF, 1, true, true)]
    [InlineData(0xE000, 0x5000, false, true)]
    public void ShouldHandleCarryFlag(ushort left, ushort right, bool carryInput, bool carryOutput) {
      Test(left, right, operands => cpu.Carry = carryInput, operands => Assert.Equal(carryOutput, cpu.Carry));
    }

  }

}

using System;
using Xunit;

namespace test.instructions {

  public class RRA : InstructionTests {
  
    void Test(string input, bool carry, Step assert) {
      cpu.registers.a = Convert.ToByte(input, 2);
      cpu.Carry = carry;
      cpu.Apply(0x1F).Do();
      assert.Invoke();
    }

    [Theory]
    [InlineData("00000000", false, "00000000")]
    [InlineData("00000000", true, "10000000")]
    [InlineData("00000001", false, "00000000")]
    [InlineData("00000001", true, "10000000")]
    [InlineData("11110000", true, "11111000")]
    [InlineData("01010101", true, "10101010")]
    public void ShouldRotateRight(string input, bool carryInput, string output) {
      Test(input, carryInput, operands => Assert.Equal(Convert.ToByte(output, 2), cpu.registers.a));
    }

    [Theory]
    [InlineData("00000000", false, false)]
    [InlineData("00000000", true, false)]
    [InlineData("00000001", false, true)]
    [InlineData("00000001", true, true)]
    [InlineData("11110000", true, false)]
    [InlineData("11010101", false, true)]
    public void ShouldCopyBit0ToCarry(string input, bool carryInput, bool carryOutput) {
      Test(input, carryInput, operands => Assert.Equal(carryOutput, cpu.Carry));
    }
  }

}

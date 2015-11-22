using System;
using Xunit;

namespace test.instructions {

  public class RLA : InstructionTests {
  
    void Test(string input, bool carry, Step assert) {
      cpu.registers.a = Convert.ToByte(input, 2);
      cpu.Carry = carry;
      cpu.Apply(0x17).Do();
      assert.Invoke();
    }

    [Theory]
    [InlineData("00000000", false, "00000000")]
    [InlineData("00000000", true, "00000001")]
    [InlineData("00000001", false, "00000010")]
    [InlineData("00000001", true, "00000011")]
    [InlineData("11110000", true, "11100001")]
    [InlineData("01010101", true, "10101011")]
    public void ShouldRotateLeft(string input, bool carryInput, string output) {
      Test(input, carryInput, operands => Assert.Equal(Convert.ToByte(output, 2), cpu.registers.a));
    }

    [Theory]
    [InlineData("00000000", false, false)]
    [InlineData("00000000", true, false)]
    [InlineData("10000000", false, true)]
    [InlineData("10000000", true, true)]
    [InlineData("11110000", true, true)]
    [InlineData("11010101", false, true)]
    public void ShouldCopyBit7ToCarry(string input, bool carryInput, bool carryOutput) {
      Test(input, carryInput, operands => Assert.Equal(carryOutput, cpu.Carry));
    }
  }

}

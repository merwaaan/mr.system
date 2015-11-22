using System;
using Xunit;

namespace test.instructions {

  public class RRCA : InstructionTests {
  
    void Test(string input, Step assert) {
      cpu.registers.a = Convert.ToByte(input, 2);
      cpu.Apply(0x0F).Do();
      assert.Invoke();
    }

    [Theory]
    [InlineData("00000000", "00000000")]
    [InlineData("00000001", "10000000")]
    [InlineData("11110000", "01111000")]
    [InlineData("01010101", "10101010")]
    public void ShouldRotateRight(string input, string output) {
      Test(input, operands => Assert.Equal(Convert.ToByte(output, 2), cpu.registers.a));
    }

    [Theory]
    [InlineData("00000000", false, false)]
    [InlineData("00000000", true, false)]
    [InlineData("00000001", false, true)]
    [InlineData("00000001", true, true)]
    [InlineData("11110000", true, false)]
    [InlineData("01010101", true, true)]
    public void ShouldCopyBit0ToCarry(string input, bool carryInput, bool carryOutput) {
      cpu.Carry = carryInput;
      Test(input, operands => Assert.Equal(carryOutput, cpu.Carry));
    }
  }

}

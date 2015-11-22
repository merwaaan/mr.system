using System;
using Xunit;

namespace test.instructions {

  public class RLCA : InstructionTests {
  
    void Test(string input, Step assert) {
      cpu.registers.a = Convert.ToByte(input, 2);
      cpu.Apply(0x07).Do();
      assert.Invoke();
    }

    [Theory]
    [InlineData("00000000", "00000000")]
    [InlineData("00000001", "00000010")]
    [InlineData("11110000", "11100001")]
    [InlineData("01010101", "10101010")]
    public void ShouldRotateLeft(string input, string output) {
      Test(input, operands => Assert.Equal(Convert.ToByte(output, 2), cpu.registers.a));
    }

    [Theory]
    [InlineData("00000000", false, false)]
    [InlineData("00000000", true, false)]
    [InlineData("00000001", false, false)]
    [InlineData("00000001", true, false)]
    [InlineData("11110000", true, true)]
    [InlineData("11010101", false, true)]
    [InlineData("11010101", true, true)]
    public void ShouldCopyBit7ToCarry(string input, bool carryInput, bool carryOutput) {
      cpu.Carry = carryInput;
      Test(input, operands => Assert.Equal(carryOutput, cpu.Carry));
    }
  }

}

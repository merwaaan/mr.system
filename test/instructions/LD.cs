using System;
using System.Linq;
using Xunit;
using mr.system;

namespace test.instructions {
  public class LD8 : InstructionTests {
    
    public LD8() {
      opcodes = Enumerable.Range(0x40, 64).Select(o => (byte) o).ToArray(); // TODO others
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    [InlineData(0x56, 0xFC)]
    [InlineData(0x12, 0x0A)]
    public void ShouldLoadValue(byte left, byte right) {
      AllOpcodes(
        operands => {
          operands[0].Target = left;
          operands[1].Target = right;
        },
        operands => Assert.Equal(operands[0].Target, right));
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    [InlineData(0x56, 0xFC)]
    [InlineData(0x12, 0x0A)]
    public void ShouldNotChangeCopiedValue(byte left, byte right) {
      AllOpcodes(
        operands => {
          operands[0].Target = left;
          operands[1].Target = right;
        },
        operands => Assert.Equal(operands[1].Target, right));
    }

  }

}

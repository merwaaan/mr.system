using System;
using Xunit;

namespace test.instructions {

  public class PUSH : InstructionTests {
  
    public PUSH() {
      opcodes = new byte[] { 0xC5, 0xD5, 0xE5, 0xF5 };
    }
    
    [Theory]
    [InlineData(0x1234, 0xFFFF)]
    [InlineData(0, 0xFF00)]
    [InlineData(0x9922, 0xF000)]
    [InlineData(0xFFFF, 0x8000)]
    public void ShouldPushOnStack(ushort input, ushort sp) {
      AllOpcodes(
        operands => {
          operands[0].Target = input;
          cpu.sp = sp;
        },
        operands => {
          Assert.Equal(input, cpu.memory.Read16((ushort) (sp - 2)));
          Assert.Equal(input & 0x00FF, cpu.memory.Read((ushort) (sp - 2)));
          Assert.Equal((input & 0xFF00) >> 8, cpu.memory.Read((ushort) (sp - 1)));
        });
    }

    [Theory]
    [InlineData(0x1234, 0xFFFF, 0xFFFD)]
    [InlineData(0, 0xFF00, 0xFEFE)]
    [InlineData(0x9922, 0xF000, 0xEFFE)]
    [InlineData(0xFFFF, 0x1234, 0x1232)]
    public void ShouldDecrementStackPointer(ushort input, ushort spInput, ushort spOutput) {
      AllOpcodes(
        operands => {
          operands[0].Target = input;
          cpu.sp = spInput;
        },
        operands => Assert.Equal(spOutput, cpu.sp));
    }

  }

}

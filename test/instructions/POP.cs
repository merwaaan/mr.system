using Xunit;

namespace test.instructions {

  public class POP : InstructionTests {
  
    public POP() {
      opcodes = new byte[] { 0xC1, 0xD1, 0xE1, 0xF1 };
    }
    
    [Theory]
    [InlineData(0x1234, 0xF000)]
    [InlineData(0, 0xFF00)]
    [InlineData(0x9922, 0x8000)]
    [InlineData(0xFFFF, 0x6789)]
    public void ShouldPopOffStack(ushort input, ushort sp) {
      AllOpcodes(
        operands => {
          cpu.sp = sp;
          cpu.memory.Write16(cpu.sp, input);
        },
        operands => {
          Assert.Equal(input, operands[0].Target);
          Assert.Equal(input & 0x00FF, ((ushort) operands[0].Target) & 0x00FF);
          Assert.Equal((input & 0xFF00) >> 8, (((ushort) operands[0].Target) & 0xFF00) >> 8);
        });
    }

    [Theory]
    [InlineData(0x1234, 0xFFFD, 0xFFFF)]
    [InlineData(0, 0xFEFE, 0xFF00)]
    [InlineData(0x9922, 0xDFFE, 0xE000)]
    [InlineData(0xFFFF, 0x1234, 0x1236)]
    public void ShouldIncrementStackPointer(ushort input, ushort spInput, ushort spOutput) {
      AllOpcodes(
        operands => {
          operands[0].Target = input;
          cpu.sp = spInput;
        },
        operands => Assert.Equal(spOutput, cpu.sp));
    }

  }

}

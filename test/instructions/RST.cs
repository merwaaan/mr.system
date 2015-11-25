using Xunit;

namespace test.instructions {

  public class RST : InstructionTests {
  
    public RST() {
      opcodes = new byte[] {
        0xC7, 0xD7, 0xE7, 0xF7,
        0xCF, 0xDF, 0xEF, 0xFF
      };
    }

    [Fact]
    public void ShouldJumpToTarget() {
      AllOpcodes(operands => Assert.Equal((byte) operands[0].Target, cpu.pc));
    }

    [Theory]
    [InlineData(0, 0xFFFF)]
    [InlineData(0x1234, 0xFF00)]
    [InlineData(0x9922, 0x9000)]
    [InlineData(0xDF00, 0xFFFF)]
    public void ShouldPushCurrentPCOnStack(ushort pc, ushort sp) {
      AllOpcodes(
        operands => {
          cpu.pc = pc;
          cpu.sp = sp;
        },
        operands => Assert.Equal(pc + 1, cpu.memory.Read16(cpu.sp)));
    }

  }

}

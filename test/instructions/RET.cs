using Xunit;
using mr.system;

namespace test.instructions {

  public class RET : InstructionTests {

    [Theory]
    [InlineData(0, 0xF000, 0, 0)]
    [InlineData(0x1234, 0xD000, 1, 1)]
    [InlineData(0xF011, 0x5678, 0xC000, 0xC000)]
    [InlineData(0xFFFF, 0x1234, 0xFFFF, 0xFFFF)]
    public void ShouldReturn(ushort pcInput, ushort sp, ushort stackTop, ushort pcOutput) {
      cpu.pc = pcInput;
      cpu.sp = sp;
      cpu.memory.Write16(cpu.sp, stackTop);
      cpu.Apply(0xC9).Do();
      Assert.Equal(pcOutput, cpu.pc);
    }

  }

  public class RET_cond : InstructionTests {

    public RET_cond() {
      opcodes = new byte[] {
        0xC0, 0xD0, 0xE0, 0xF0,
        0xC8, 0xD8, 0xE8, 0xF8
      };
    }

    [Theory]
    [InlineData(0, 0xF000, 0, true, 0)]
    [InlineData(0, 0xF000, 0, false, 1)]
    [InlineData(0x1234, 0xD000, 1, true, 1)]
    [InlineData(0x1234, 0xD000, 1, false, 0x1235)]
    [InlineData(0xF011, 0x5678, 0xC000, true, 0xC000)]
    [InlineData(0xF011, 0x5678, 0xC000, false, 0xF012)]
    [InlineData(0xFFFF, 0x1234, 0xFFFF, true, 0xFFFF)]
    [InlineData(0xFFFF, 0x1234, 0xFFFF, false, 0)]
    public void ShouldReturnDependingOnFlag(ushort pcInput, ushort sp, ushort stackTop, bool conditionMet, ushort pcOutput) {
      AllOpcodes(
        operands => {
          cpu.pc = pcInput;
          cpu.sp = sp;
          cpu.memory.Write16(cpu.sp, stackTop);
          operands[0].Target = conditionMet;
        },
        operands => Assert.Equal(pcOutput, cpu.pc));
    }
  }
}

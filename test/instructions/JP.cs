using Xunit;
using mr.system;

namespace test.instructions {

  public class JP : InstructionTests {

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 1)]
    [InlineData(0, 0x1234)]
    [InlineData(0, 0xFFFF)]
    [InlineData(0x1234, 0)]
    [InlineData(0xFF00, 0x3542)]
    public void ShouldJumpToTarget(ushort pcInput, ushort pcTarget) {
      cpu.pc = pcInput;
      Instruction call = cpu.Apply(0xC3);
      call.Operands[0].Target = pcTarget;
      call.Do();
      Assert.Equal(pcTarget, cpu.pc);
    }

  }

  public class JP_cond : InstructionTests {

    public JP_cond() {
      opcodes = new byte[] {
        0xC2, 0xD2, 0xE2, 0xF2,
        0xCA, 0xDA, 0xEA, 0xFA
      };
    }
    
    [Theory]
    [InlineData(0, 0, false, 3)]
    [InlineData(0, 0, true, 0)]
    [InlineData(0, 1, false, 3)]
    [InlineData(0, 1, true, 1)]
    [InlineData(0x1000, 0x1234, true, 0x1234)]
    [InlineData(0x0666, 0xFFFF, false, 0x0669)]
    [InlineData(0x1234, 0, true, 0)]
    public void ShouldJumpToTargetDependingOnFlag(ushort pcInput, ushort pcTarget, bool conditionMet, ushort pcOutput) {
      AllOpcodes(
        operands => {
          cpu.pc = pcInput;
          operands[1].Target = pcTarget;
          operands[0].Target = conditionMet;
        },
        operands => Assert.Equal(pcOutput, cpu.pc));
    }

  }
}

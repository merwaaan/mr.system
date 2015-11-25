using Xunit;
using mr.system;

namespace test.instructions {

  public class CALL : InstructionTests {

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 1)]
    [InlineData(0, 0x1234)]
    [InlineData(0, 0xFFFF)]
    [InlineData(0x1234, 0)]
    [InlineData(0xFF00, 0x3542)]
    public void ShouldJumpToTarget(ushort pcInput, ushort pcTarget) {
      cpu.pc = pcInput;
      Instruction call = cpu.Apply(0xCD);
      call.Operands[0].Target = pcTarget;
      call.Do();
      Assert.Equal(pcTarget, cpu.pc);
    }

    [Theory]
    [InlineData(0, 0xAAAA, 0xFFFF, 3)]
    [InlineData(0, 0xFFFF, 0xFFFF, 3)]
    [InlineData(0x1234, 0xFF00, 0x8000, 0x1237)]
    [InlineData(0x9922, 0x9000, 0x1234, 0x9925)]
    [InlineData(0xDF00, 0xFFFF, 0xDF00, 0xDF03)]
    [InlineData(0xFFFF, 0x5000, 0xFFFF, 0x0002)]
    public void ShouldPushCurrentPCOnStack(ushort pcInput, ushort pcTarget, ushort sp, ushort stackTopOutput) {
      cpu.pc = pcInput;
      cpu.sp = sp;
      Instruction call = cpu.Apply(0xCD);
      call.Operands[0].Target = pcTarget;
      call.Do();
      Assert.Equal(stackTopOutput, cpu.memory.Read16(cpu.sp));
      Assert.Equal(sp - 2, cpu.sp);
    }

  }

  public class CALL_cond : InstructionTests {

    public CALL_cond() {
      opcodes = new byte[] {
        0xC4, 0xD4, 0xE4, 0xF4,
        0xCC, 0xDC, 0xEC, 0xFC
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

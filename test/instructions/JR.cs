using Xunit;
using mr.system;

namespace test.instructions {

  public class JR : InstructionTests {

    [Theory]
    [InlineData(0, 3, 5)]
    [InlineData(0, 0, 2)]
    [InlineData(0x0480, 3, 0x0485)]
    [InlineData(0x1234, 0x54, 0x128A)]
    [InlineData(0x80, 0x80, 2)]
    [InlineData(0x100, 0x80, 0x82)]
    [InlineData(0xFFFF, 0, 1)]
    [InlineData(0xFFFF, 1, 2)]
    [InlineData(0xFFFF, 2, 3)]
    public void ShouldJumpToTarget(ushort pcInput, byte offset, ushort pcOutput) {
      cpu.pc = pcInput;
      Instruction jr = cpu.Apply(0x18);
      jr.Operands[0].Target = offset;
      jr.Do();
      Assert.Equal(pcOutput, cpu.pc);
    }

  }

  public class JR_cond : InstructionTests {

    public JR_cond() {
      opcodes = new byte[] { 0x20, 0x30, 0x28, 0x38 };
    }
    
    [Theory]
    [InlineData(0, 3, false, 2)]
    [InlineData(0, 3, true, 5)]
    [InlineData(0, 0, false, 2)]
    [InlineData(0, 0, true, 2)]
    [InlineData(0x0480, 3, false, 0x0482)]
    [InlineData(0x0480, 3, true, 0x0485)]
    [InlineData(0x1234, 0x54, false, 0x1236)]
    [InlineData(0x1234, 0x54, true, 0x128A)]
    [InlineData(0x80, 0x80, false, 0x82)]
    [InlineData(0x80, 0x80, true, 2)]
    [InlineData(0x100, 0x80, false, 0x102)]
    [InlineData(0x100, 0x80, true, 0x82)]
    [InlineData(0xFFFF, 0, false, 1)]
    [InlineData(0xFFFF, 0, true, 1)]
    [InlineData(0xFFFF, 1, false, 1)]
    [InlineData(0xFFFF, 1, true, 2)]
    [InlineData(0xFFFF, 2, false, 1)]
    [InlineData(0xFFFF, 2, true, 3)]
    public void ShouldJumpToTargetDependingOnFlag(ushort pcInput, byte offset, bool conditionMet, ushort pcOutput) {
      AllOpcodes(
        operands => {
          cpu.pc = pcInput;
          operands[1].Target = offset;
          operands[0].Target = conditionMet;
        },
        operands => Assert.Equal(pcOutput, cpu.pc));
    }

  }
}

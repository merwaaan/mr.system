using System;
using Xunit;
using mr.system;

namespace test.instructions {
  public class InstructionTests {

    protected CPU cpu;
    protected byte[] opcodes;
    
    protected InstructionTests() {
      cpu = new CPU(new Memory());
      opcodes = null;
    }

    protected void Execute() { }

    protected delegate void Step(params Operand[] operands);

    protected void AllOpcodes(Step assert) {
      foreach (byte opcode in opcodes) {
        Instruction instruction = cpu.Apply(opcode);
        Operand[] operands = instruction.Operands;
        instruction.Do();
        assert(operands);
      }
    }

    protected void AllOpcodes(Step setup, Step assert) {
      foreach (byte opcode in opcodes) {
        Instruction instruction = cpu.Apply(opcode);
        Operand[] operands = instruction.Operands;
        setup(operands);
        instruction.Do();
        assert(operands);
      }
    }

    protected void AllByteValues(Action<byte> action) {
      for (byte b = 0; b < 0xFF; ++b)
        action.Invoke(b);
    }

    protected void AllBoolValues(Action<bool> action) {
      action.Invoke(false);
      action.Invoke(true);
    }

    protected void AllByteBoolValues(Action<byte, bool> action) {
      AllByteValues(b => AllBoolValues(f => action.Invoke(b, f)));
    }
    
  }

}

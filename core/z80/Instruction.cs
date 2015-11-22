using System;
using System.Reflection;
using System.Linq;

namespace mr.system {

  public class Instruction {

    public struct Info {
      public byte opcode;
      public string mnemonics;
      public string description;
      public int size;
      public int cycles;
    }

    readonly CPU cpu;
    readonly Action implementation;
    public readonly Info info;

    public Operand[] Operands {
      get {
        FieldInfo[] fields = implementation.Method.DeclaringType.GetFields();
        return fields.Where(f => f.GetValue(implementation.Target) is Operand)
                     .Select(f => (Operand) f.GetValue(implementation.Target))
                     .ToArray();
      }
    }

    public Instruction(CPU cpu, byte opcode, string mnemonics, string description, int size, int cycles, Action implementation) {
      this.cpu = cpu;

      info = new Info {
        opcode = opcode,
        mnemonics = mnemonics,
        description  = description,
        size = size,
        cycles = cycles
      };

      this.implementation = implementation;
    }
    
    public void Do() {
      // Execute the implementation
      implementation();

      // Move the program counter
      cpu.pc += (ushort) info.size;
    }

    public override string ToString() {
      return PrintInfo();
    }

    public string PrintInfo() {
      return $"{info.opcode.ToString("X2")}: {info.mnemonics} ({info.description})";
    }

    public string PrintDisassembled() {
      string opcode  = string.Join(" ", Enumerable.Range(cpu.pc, info.size)
                                                  .Select(addr => cpu.memory.Read(addr).ToString("X2")));

      return $"{opcode}: {info.mnemonics}";
    }
  }
  
  partial class Instructions {

    public static Action nop(CPU cpu) =>
      () => { };

    public static Action halt(CPU cpu) =>
      () => { }; // TODO

    public static Action scf(CPU cpu) =>
      () => {
        cpu.HalfCarry = false;
        cpu.AddSub = false;
        cpu.Carry = true;
      };

    public static Action ccf(CPU cpu) =>
      () => {
        cpu.HalfCarry = cpu.Carry;
        cpu.AddSub = false;
        cpu.Carry = !cpu.Carry;
      };

    public static Action di(CPU cpu) =>
      () => cpu.interruptsEnabled = false;

    public static Action ei(CPU cpu) =>
      () => cpu.interruptsEnabled = true;

    public static Action im(CPU cpu, Fixed o) =>
      () => cpu.interruptMode = o.Target;
    
    public static Action ld(CPU cpu, Operand<byte> dest, Operand<byte> source) =>
      () => dest.Target = source.Target;

    public static Action ld(CPU cpu, Operand<ushort> dest, Operand<ushort> source) =>
      () => dest.Target = source.Target;

    public static Action ex(CPU cpu, Register16 o1, Register16 o2) =>
      () => {
        ushort de = cpu.registers.de; // TODO use o1, o2?
        cpu.registers.de = cpu.registers.hl;
        cpu.registers.hl = de;
      }; 
      
    public static Action ex(CPU cpu, Indirect8 o1, Register16 o2) =>
      () => {
        // TODO
      };

    public static Action exx(CPU cpu) =>
      () => {
        CPU.Registers regs = cpu.registers;
        cpu.registers = cpu.altRegisters;
        cpu.altRegisters = regs;
        ushort af = cpu.registers.af;
        cpu.registers.af = cpu.altRegisters.af;
        cpu.altRegisters.af = af;
      };

    public static Action ldi(CPU cpu) =>
      () => { }; // TODO
    public static Action ldir(CPU cpu) =>
      () => { }; // TODO

    public static Action cpi(CPU cpu) =>
      () => { }; // TODO
    public static Action cpir(CPU cpu) =>
      () => { }; // TODO

    public static Action ini(CPU cpu) =>
      () => { }; // TODO
    public static Action inir(CPU cpu) =>
      () => { }; // TODO

    public static Action outi(CPU cpu) =>
      () => { }; // TODO
    public static Action otir(CPU cpu) =>
      () => { }; // TODO

    public static Action ldd(CPU cpu) =>
      () => { }; // TODO
    public static Action lddr(CPU cpu) =>
      () => { }; // TODO

    public static Action cpd(CPU cpu) =>
      () => { }; // TODO
    public static Action cpdr(CPU cpu) =>
      () => { }; // TODO

    public static Action ind(CPU cpu) =>
      () => { }; // TODO
    public static Action indr(CPU cpu) =>
      () => { }; // TODO

    public static Action outd(CPU cpu) =>
      () => { }; // TODO
    public static Action otdr(CPU cpu) =>
      () => { }; // TODO

  }
}

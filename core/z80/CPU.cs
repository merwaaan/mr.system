using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace mr.system {
  public class CPU {
    
    [StructLayout(LayoutKind.Explicit)]
    public struct Registers {
      [FieldOffset(0)] public byte f;
      [FieldOffset(1)] public byte a;
      [FieldOffset(0)] public ushort af;
      
      [FieldOffset(2)] public byte c;
      [FieldOffset(3)] public byte b;
      [FieldOffset(2)] public ushort bc;

      [FieldOffset(4)] public byte e;
      [FieldOffset(5)] public byte d;
      [FieldOffset(4)] public ushort de;
      
      [FieldOffset(6)] public byte l;
      [FieldOffset(7)] public byte h;
      [FieldOffset(6)] public ushort hl;
    }

    public Registers registers;
    public Registers altRegisters;

    public bool Carry {
      get { return Utils.Bit(registers.f, 0); }
      set { registers.f = Utils.Bit(registers.f, 0, value); }
    }

    public bool AddSub {
      get { return Utils.Bit(registers.f, 1); }
      set { registers.f = Utils.Bit(registers.f, 1, value); }
    }

    public bool Overflow {
      get { return Utils.Bit(registers.f, 2); }
      set { registers.f = Utils.Bit(registers.f, 2, value); }
    }

    public bool HalfCarry {
      get { return Utils.Bit(registers.f, 4); }
      set { registers.f = Utils.Bit(registers.f, 4, value); }
    }

    public bool Zero {
      get { return Utils.Bit(registers.f, 6); }
      set { registers.f = Utils.Bit(registers.f, 6, value); }
    }

    public bool Sign {
      get { return Utils.Bit(registers.f, 7); }
      set { registers.f = Utils.Bit(registers.f, 7, value); }
    }

    public ushort pc;
    public ushort sp;

    public bool interruptsEnabled;
    public int interruptMode; // TODO prop with bound checks
    
    public readonly Memory memory;
    public readonly InstructionSet instructions;

    public CPU(Memory memory) {
      this.memory = memory;
      instructions = new InstructionSet(this);
    }

    public byte AtPC(int offset = 0) {
      return memory.Read(pc + offset);
    }

    public override string ToString() {
      return $"PC={pc.ToString("X2")} " + 
             $"AF={registers.af.ToString("X2")} " +
             $"BC={registers.bc.ToString("X2")} " +
             $"DE={registers.de.ToString("X2")} " +
             $"HL={registers.hl.ToString("X2")} " +
             $"SP={sp.ToString("X2")} ";
    }

    public void Step() {

      try {
        Instruction instruction = instructions.GetCurrent();

        Console.WriteLine(instruction.ToString());
        Console.WriteLine(instructions.Disassembled());
        Console.WriteLine(ToString());
        instruction.Do();
        Console.WriteLine(ToString());
      }
      catch (InstructionSet.UnimplementedInstructionException e) {
        Console.WriteLine(e);
      }
    }

    public Instruction Apply(byte instr) { // TODO clean up (for testing only)
      memory.Write(0, instr);
      pc = 0;
      return instructions.GetCurrent();
    }

    public void Jump(ushort address) {
      pc = address;
    }

    public void Push(ushort value) {
      sp -= 2;
      memory.Write16(sp, value);
    }

    public ushort Pop() {
      ushort value = memory.Read16(sp);
      sp += 2;
      return value;
    }

    public void Call(ushort address) {
      Push((ushort) (pc + 3));
      Jump((ushort) (address - 3));
    }

    public void Return() {
      Jump(Pop());
    }
  }
}

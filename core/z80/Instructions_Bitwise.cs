using System;

namespace mr.system {

  partial class Instructions {

    public static Action bit(CPU cpu, Fixed bit, Operand<byte> value) =>
      () => {
        cpu.Zero = Utils.Bit(value.Target, bit.Target);
        cpu.HalfCarry = true;
        cpu.AddSub = false;
      };

    public static Action set(CPU cpu, Fixed bit, Operand<byte> value) =>
      () => value.Target = Utils.SetBit(value.Target, bit.Target);

    public static Action res(CPU cpu, Fixed bit, Operand<byte> value) =>
      () => value.Target = Utils.SetBit(value.Target, bit.Target);

    public static Action rlca(CPU cpu) =>
      () => {
        bool bit7 = Utils.Bit(cpu.registers.a, 7);
        cpu.registers.a <<= 1;
        cpu.registers.a = Utils.Bit(cpu.registers.a, 0, bit7);
        cpu.HalfCarry = false;
        cpu.AddSub = false;
        cpu.Carry = bit7;
      };

    public static Action rla(CPU cpu) =>
      () => {
        bool bit7 = Utils.Bit(cpu.registers.a, 7);
        cpu.registers.a <<= 1;
        cpu.registers.a = Utils.Bit(cpu.registers.a, 0, cpu.Carry);
        cpu.HalfCarry = false;
        cpu.AddSub = false;
        cpu.Carry = bit7;
      };

    public static Action rrca(CPU cpu) =>
      () => {
        bool bit0 = Utils.Bit(cpu.registers.a, 0);
        cpu.registers.a >>= 1;
        cpu.registers.a = Utils.Bit(cpu.registers.a, 7, bit0);
        cpu.HalfCarry = false;
        cpu.AddSub = false;
        cpu.Carry = bit0;
      };

    public static Action rra(CPU cpu) =>
      () => {
        bool bit0 = Utils.Bit(cpu.registers.a, 0);
        cpu.registers.a >>= 1;
        cpu.registers.a = Utils.Bit(cpu.registers.a, 7, cpu.Carry);
        cpu.HalfCarry = false;
        cpu.AddSub = false;
        cpu.Carry = bit0;
      };
  }

}

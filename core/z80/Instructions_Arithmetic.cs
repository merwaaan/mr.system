using System;

namespace mr.system {

  partial class Instructions {

    public static Action inc(CPU cpu, Operand<byte> o) =>
      () => {
        ++o.Target;
        cpu.Sign = o.Target > 0x7F;
        cpu.Zero = o.Target == 0;
        cpu.HalfCarry = (o.Target & 0x0F) == 0;
        cpu.Overflow = o.Target == 0x80;
        cpu.AddSub = false;
      };

    public static Action inc(CPU cpu, Register16 o) =>
      () => ++o.Target;

    public static Action dec(CPU cpu, Operand<byte> o) =>
      () => {
        --o.Target;
        cpu.Sign = o.Target > 0x7F;
        cpu.Zero = o.Target == 0;
        cpu.HalfCarry = (o.Target & 0x0F) == 0x0F;
        cpu.Overflow = o.Target == 0x7F;
        cpu.AddSub = true;
      };

    public static Action dec(CPU cpu, Register16 o) =>
      () => --o.Target;

    public static Action add(CPU cpu, Register8 o1, Operand<byte> o2) =>
      () => {
        byte a = o1.Target;
        o1.Target += o2.Target;
        cpu.Sign = o1.Target > 0x7F;
        cpu.Zero = o1.Target == 0;
        cpu.HalfCarry = (a & 0x0F) + (o2.Target & 0x0F) > 0x0F;
        cpu.Overflow = Utils.Sign(a) == Utils.Sign(o2.Target) &&
                       Utils.Sign(a) != Utils.Sign(o1.Target);
        cpu.AddSub = false; 
        cpu.Carry = a + o2.Target > 0xFF;
      };

    public static Action add(CPU cpu, Register16 o1, Register16 o2) =>
      () => {
        ushort hl = o1.Target;
        o1.Target += o2.Target;
        cpu.HalfCarry = (hl & 0xFFF) + (o2.Target & 0xFFF) > 0xFFF;
        cpu.AddSub = false;
        cpu.Carry = hl + o2.Target > 0xFFFF;
      };

    public static Action adc(CPU cpu, Register8 o1, Operand<byte> o2) =>
      () => {
        cpu.registers.a += (byte)(o2.Target + Convert.ToByte(cpu.Carry));
        cpu.Sign = cpu.registers.a > 0x7F;
        cpu.Zero = cpu.registers.a == 0;
        cpu.HalfCarry = true; // TODO
        cpu.Overflow = true; // TODO
        cpu.AddSub = false;
        cpu.Carry = false; // TODO
      };

    public static Action adc(CPU cpu, Register16 o1, Register16 o2) =>
      () => {
        cpu.registers.hl += (ushort)(o2.Target + Convert.ToByte(cpu.Carry));
        cpu.Sign = cpu.registers.a > 0x7F;
        cpu.Zero = cpu.registers.a == 0;
        cpu.HalfCarry = true; // TODO
        cpu.Overflow = true; // TODO
        cpu.AddSub = false;
        cpu.Carry = false; // TODO
      };

    public static Action sub(CPU cpu, Operand<byte> o2) =>
      () => {
        cpu.registers.a -= o2.Target;
        cpu.Sign = cpu.registers.a > 0x7F;
        cpu.Zero = cpu.registers.a == 0;
        cpu.HalfCarry = true; // TODO
        cpu.Overflow = true; // TODO
        cpu.AddSub = true;
        cpu.Carry = false; // TODO
      };

    public static Action sbc(CPU cpu, Register8 o1, Operand<byte> o) =>
      () => {
        cpu.registers.a -= o.Target;
        cpu.Sign = cpu.registers.a > 0x7F;
        cpu.Zero = cpu.registers.a == 0;
        cpu.HalfCarry = true; // TODO
        cpu.Overflow = true; // TODO
        cpu.AddSub = true;
        cpu.Carry = false; // TODO
      };

    public static Action sbc(CPU cpu, Register16 o1, Operand<ushort> o) =>
      () => {
        cpu.registers.hl -= o.Target;
        cpu.Sign = cpu.registers.hl > 0x7F;
        cpu.Zero = cpu.registers.hl == 0;
        cpu.HalfCarry = true; // TODO
        cpu.Overflow = true; // TODO
        cpu.AddSub = true;
        cpu.Carry = false; // TODO
      };

    public static Action cp(CPU cpu, Operand<byte> o) =>
      () => {
        cpu.Sign = cpu.registers.a > o.Target;
        cpu.Zero = cpu.registers.a == o.Target;
        cpu.HalfCarry = true; // TODO h4
        cpu.Overflow = true; // TODO overflow
        cpu.AddSub = true;
        cpu.Carry = false; // TODO borrow
      };

    public static Action and(CPU cpu, Operand<byte> o) =>
      () => {
        cpu.registers.a &= o.Target;
        cpu.Sign = cpu.registers.a > 0x7F;
        cpu.Zero = cpu.registers.a == 0;
        cpu.HalfCarry = true;
        cpu.Overflow = Utils.Parity(cpu.registers.a);
        cpu.AddSub = false;
        cpu.Carry = false;
      };

    public static Action or(CPU cpu, Operand<byte> o) =>
      () => {
        cpu.registers.a |= o.Target;
        cpu.Sign = cpu.registers.a > 0x7F;
        cpu.Zero = cpu.registers.a == 0;
        cpu.HalfCarry = false;
        cpu.Overflow = Utils.Parity(cpu.registers.a);
        cpu.AddSub = false;
        cpu.Carry = false;
      };

    public static Action xor(CPU cpu, Operand<byte> o) =>
      () => {
        cpu.registers.a ^= o.Target;
        cpu.Sign = cpu.registers.a > 0x7F;
        cpu.Zero = cpu.registers.a == 0;
        cpu.HalfCarry = false;
        cpu.Overflow = Utils.Parity(cpu.registers.a);
        cpu.AddSub = false;
        cpu.Carry = false;
      };

    public static Action cpl(CPU cpu) =>
      () => {
        cpu.registers.a = (byte) ~cpu.registers.a;
        cpu.HalfCarry = true;
        cpu.AddSub = true;
      };
  }

}

using System;

namespace mr.system {

  partial class Instructions {

    public static Action push(CPU cpu, Register16 o) =>
      () => cpu.Push(o.Target);

    public static Action pop(CPU cpu, Register16 o) =>
      () => o.Target = cpu.Pop();

    public static Action jr(CPU cpu, Operand<byte> o) =>
      () => cpu.pc += (ushort) Utils.Signed(o.Target);

    public static Action jr(CPU cpu, FlagCondition o1, Operand<byte> o2) =>
      () => {
        if (o1.Target)
          cpu.pc = (ushort) (cpu.pc + Utils.Signed(o2.Target));
      };
    
    public static Action djnz(CPU cpu, Immediate8 o) =>
      () => {
        if (--cpu.registers.b == 0)
          cpu.pc += o.Target;
      };

    public static Action jp(CPU cpu, Operand<ushort> o) =>
      () => cpu.pc = (ushort) (o.Target - 3); // TODO handle adjustment in Instr

    public static Action jp(CPU cpu, FlagCondition o1, Operand<ushort> o2) =>
      () => {
        if (o1.Target)
          cpu.pc = (ushort) (o2.Target - 3);
      };

    public static Action call(CPU cpu, Immediate<ushort> o) =>
      () => cpu.Call(o.Target, 3);

    public static Action call(CPU cpu, FlagCondition o1, Immediate<ushort> o2) =>
      () => {
        if (o1.Target)
          cpu.Call(o2.Target, 3);
      };

    public static Action ret(CPU cpu) =>
      () => {
        cpu.Return();
        cpu.pc -= 1;
      };// TODO handle that in Instruc...

    public static Action ret(CPU cpu, FlagCondition o) =>
      () => {
        if (o.Target) {
          cpu.Return();
          cpu.pc -= 1;
        }
      };

    public static Action rst(CPU cpu, Fixed o) =>
      () => cpu.Call(o.Target, 1);
  }

}

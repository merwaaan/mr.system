using System;
using System.Runtime.InteropServices;

namespace mr.system {

  public interface Operand {
    object Target { get; set; }
  }

  public abstract class Operand<T> : Operand where T : struct {
  
    protected Func<T> getter;
    protected Action<T> setter;

    public T Target {
      get { return getter(); }
      set { setter(value); }
    }

    object Operand.Target {
      get {
        return Target;
      }
      set {
        Target = (T) value;
      }
    }

    public override string ToString() {
      return string.Format($"0x{{0:X{Marshal.SizeOf(typeof(T)) * 2}}}", Target);
    }
  }

  public class Fixed : Operand<byte> {
    
    public Fixed(byte value) {
      getter = () => value;
      setter = (x) => {};
    }
  }
  
  public class FlagCondition : Operand<bool> {

    public FlagCondition(CPU cpu, string flagName, bool state) {

      setter = (x) => { }; // TODO exception?

      switch (flagName) {
        case "carry": getter = () => cpu.Carry == state; break;
        case "zero": getter = () => cpu.Zero == state; break;
        case "parity": getter = () => cpu.Overflow == state; break;
        case "sign": getter = () => cpu.Sign == state; break;
        default:
          throw new NotImplementedException($"Flag {flagName} does not exist");
      }
    }

    public override string ToString() {
      return Target ? "1" : "0";
    }
  }

  public class Indirect8 : Operand<byte> {
    public Indirect8(CPU cpu, Operand<ushort> sub) {
      getter = () => cpu.memory.Read(sub.Target);
      setter = (x) => cpu.memory.Write(sub.Target, x);
    }
  }

  public class Indirect16 : Operand<ushort> {
    public Indirect16(CPU cpu, Operand<ushort> sub) {
      getter = () => cpu.memory.Read16(sub.Target);
      setter = (x) => cpu.memory.Write16(sub.Target, x);
    }
  }

  public abstract class Immediate<T> : Operand<T> where T : struct {}
  
  public class Immediate8 : Immediate<byte> { // TODO unit tests
    public Immediate8(CPU cpu) {
      getter = () => cpu.memory.Read(cpu.pc + 1); // TODO handle position
      setter = (x) => cpu.memory.Write(cpu.pc + 1, x);
    }
  }

  public class Immediate16 : Immediate<ushort> { // TODO unit tests
    public Immediate16(CPU cpu) {
      getter = () => cpu.memory.Read16(cpu.pc + 1);
      setter = (x) => cpu.memory.Write16(cpu.pc + 1, x);
    }
  }
  
  public class Register8 : Operand<byte> {
    
    public Register8(CPU cpu, string registerName) {
      switch (registerName) {
        case "a":
          getter = () => cpu.registers.a;
          setter = (x) => cpu.registers.a = x;
          break;
        case "f":
          getter = () => cpu.registers.f;
          setter = (x) => cpu.registers.f = x;
          break;
        case "b":
          getter = () => cpu.registers.b;
          setter = (x) => cpu.registers.b = x;
          break;
        case "c":
          getter = () => cpu.registers.c;
          setter = (x) => cpu.registers.c = x;
          break;
        case "d":
          getter = () => cpu.registers.d;
          setter = (x) => cpu.registers.d = x;
          break;
        case "e":
          getter = () => cpu.registers.e;
          setter = (x) => cpu.registers.e = x;
          break;
        case "h":
          getter = () => cpu.registers.h;
          setter = (x) => cpu.registers.h = x;
          break;
        case "l":
          getter = () => cpu.registers.l;
          setter = (x) => cpu.registers.l = x;
          break;
        default:
          throw new NotImplementedException($"Register {registerName} does not exist");
      }
    }
  }

  public class Register16 : Operand<ushort> {

    public Register16(CPU cpu, string registerName) {
      switch (registerName) {
        case "af":
          getter = () => cpu.registers.af;
          setter = (x) => cpu.registers.af = x;
          break;
        case "bc":
          getter = () => cpu.registers.bc;
          setter = (x) => cpu.registers.bc = x;
          break;
        case "de":
          getter = () => cpu.registers.de;
          setter = (x) => cpu.registers.de = x;
          break;
        case "hl":
          getter = () => cpu.registers.hl;
          setter = (x) => cpu.registers.hl = x;
          break;
        case "sp":
          getter = () => cpu.sp;
          setter = (x) => cpu.sp = x;
          break;
        default:
          throw new NotImplementedException($"Register pair {registerName} does not exist");
      }
    }
  }
}

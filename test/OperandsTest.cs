using System;
using Xunit;
using mr.system;

namespace test {
  public class OperandsTest {

    public class FixedTest {
      CPU cpu = new CPU(null);

      [Fact]
      public void CanGetValue() {
        Fixed f = new Fixed(0x35);
        Assert.Equal(0x35, f.Target);
      }

      [Fact]
      public void CannotChangeValue() {
        Fixed f = new Fixed(0x50);
        f.Target = 0xFF;
        Assert.Equal(0x50, f.Target);
      }
    }

    public class Register8Test {
      CPU cpu = new CPU(null);

      [Fact]
      public void CanGetOriginalValue() {
        cpu.registers.a = 0x05;
        Register8 reg = new Register8(cpu, "a");
        Assert.Equal(0x05, reg.Target);
      }

      [Fact]
      public void CanGetChangedValue() {
        cpu.registers.a = 0x05;
        Register8 reg = new Register8(cpu, "a");
        cpu.registers.a = 0x10;
        Assert.Equal(0x10, reg.Target);
      }

      [Fact]
      public void CanChangeValue() {
        cpu.registers.a = 0;
        Register8 reg = new Register8(cpu, "a");
        reg.Target = 0xFF;
        Assert.Equal(0xFF, reg.Target);
        Assert.Equal(0xFF, cpu.registers.a);
      }

      public class Register16Test {
        CPU cpu = new CPU(null);

        [Fact]
        public void CanGetOriginalValue() {
          cpu.registers.af = 0x0506;
          Register16 reg = new Register16(cpu, "af");
          Assert.Equal(0x0506, reg.Target);
        }

        [Fact]
        public void CanGetChangedValue() {
          cpu.registers.af = 0x0510;
          Register16 reg = new Register16(cpu, "af");
          cpu.registers.af = 0x1011;
          Assert.Equal(0x1011, reg.Target);
        }

        [Fact]
        public void CanChangeValue() {
          cpu.registers.af = 0;
          Register16 reg = new Register16(cpu, "af");
          reg.Target = 0xFFF0;
          Assert.Equal(0xFFF0, reg.Target);
          Assert.Equal(0xFFF0, cpu.registers.af);
        }
      }
    }

    public class FlagConditionTest {
      CPU cpu = new CPU(null);

      [Theory]
      [InlineData(true, true, true)]
      [InlineData(false, false, true)]
      [InlineData(true, false, false)]
      [InlineData(false, true, false)]
      public void CanCheckOriginalValue(bool flag, bool check, bool output) {
        cpu.Carry = flag;
        var f = new FlagCondition(cpu, "carry", check);
        Assert.Equal(output, f.Target);
      }
      
      // TODO...
    }
  }
}

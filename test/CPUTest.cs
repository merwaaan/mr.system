using Xunit;
using mr.system;

namespace test {
  public class CPUTest {

    public class Registers {
      CPU cpu = new CPU(null);

      [Fact]
      public void EndiannessAF() {
        cpu.registers.af = 0x10FF;
        Assert.Equal(0x10, cpu.registers.a);
        Assert.Equal(0xFF, cpu.registers.f);
        Assert.Equal(0x10FF, cpu.registers.af);
      }

      [Fact]
      public void EndiannessBC() {
        cpu.registers.bc = 0x6667;
        Assert.Equal(0x66, cpu.registers.b);
        Assert.Equal(0x67, cpu.registers.c);
        Assert.Equal(0x6667, cpu.registers.bc);
      }

      [Fact]
      public void EndiannessDE() {
        cpu.registers.de = 0x0123;
        Assert.Equal(0x01, cpu.registers.d);
        Assert.Equal(0x23, cpu.registers.e);
        Assert.Equal(0x0123, cpu.registers.de);
      }

      [Fact]
      public void EndiannessHL() {
        cpu.registers.hl = 0x3210;
        Assert.Equal(0x32, cpu.registers.h);
        Assert.Equal(0x10, cpu.registers.l);
        Assert.Equal(0x3210, cpu.registers.hl);
      }
    }


    public class Flags {
      CPU cpu = new CPU(null);

      [Theory]
      [InlineData(true, true)]
      [InlineData(false, false)]
      public void Carry(bool state, bool output) {
        cpu.Carry = state;
        Assert.Equal(output, cpu.Carry);
      }

      [Theory]
      [InlineData(true, true)]
      [InlineData(false, false)]
      public void AddSub(bool state, bool output) {
        cpu.AddSub = state;
        Assert.Equal(output, cpu.AddSub);
      }

      [Theory]
      [InlineData(true, true)]
      [InlineData(false, false)]
      public void Overflow(bool state, bool output) {
        cpu.Overflow = state;
        Assert.Equal(output, cpu.Overflow);
      }

      [Theory]
      [InlineData(true, true)]
      [InlineData(false, false)]
      public void HalfCarry(bool state, bool output) {
        cpu.HalfCarry = state;
        Assert.Equal(output, cpu.HalfCarry);
      }

      [Theory]
      [InlineData(true, true)]
      [InlineData(false, false)]
      public void Zero(bool state, bool output) {
        cpu.Zero = state;
        Assert.Equal(output, cpu.Zero);
      }

      [Theory]
      [InlineData(true, true)]
      [InlineData(false, false)]
      public void Sign(bool state, bool output) {
        cpu.Sign = state;
        Assert.Equal(output, cpu.Sign);
      }

    }
  }
}

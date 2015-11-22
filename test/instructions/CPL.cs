using System;
using Xunit;
using mr.system;

namespace test.instructions {
  public class CPL : InstructionTests {

    [Theory]
    [InlineData(0, 0xFF)]
    [InlineData(0xFF, 0)]
    [InlineData(0xF0, 0x0F)]
    [InlineData(0x0F, 0xF0)]
    [InlineData(0xAA, 0x55)]
    public void ShouldComplementRegisters(byte input, byte output) {
      cpu.registers.a = input;
      cpu.Apply(0x2F).Do();
      Assert.Equal(output, cpu.registers.a);
    }

    [Theory]
    [InlineData(0, 0, 0x12)]
    [InlineData(0, 0xFF, 0xFF)]
    [InlineData(0x3E, 0xD2, 0xD2)]
    [InlineData(0x99, 0xD0, 0xD2)]
    public void ShouldHandleFlags(byte input, byte flagsIn, byte flagsOut) {
      // Should only set halfcarry and addsub flags
      cpu.registers.a = input;
      cpu.registers.f = flagsIn;
      cpu.Apply(0x2F).Do();
      Assert.Equal(flagsOut, cpu.registers.f);
    }
    
  }
}

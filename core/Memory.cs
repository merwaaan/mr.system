﻿using System;

namespace mr.system {
  public class Memory {

    private byte[] data;

    public Memory() {
      this.data = new byte[0x10000]; // TODO should do that??
    }

    public void Load(byte[] d) {
      data = d;
    }

    public byte Read(ushort address) {
      return data[address];
    }

    public ushort Read16(ushort address) {
      return (ushort) (data[address] | data[address + 1] << 8);
    }

    public void Write(ushort address, byte value) {
      data[address] = value;
    }
    
    public void Write16(ushort address, ushort value) {
      data[address] = (byte) (value & 0x00FF);
      data[address + 1] = (byte) ((value & 0xFF00) >> 8);
    }
  }
}

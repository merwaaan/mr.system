﻿using System;

namespace mr.system {
  public class Utils {

    static bool[] parityLookup = new bool[] {
      true, false, false, true, false, true, true, false, false, true, true, false, true, false, false, true,
      false, true, true, false, true, false, false, true, true, false, false, true, false, true, true, false,
      false, true, true, false, true, false, false, true, true, false, false, true, false, true, true, false,
      true, false, false, true, false, true, true, false, false, true, true, false, true, false, false, true,
      false, true, true, false, true, false, false, true, true, false, false, true, false, true, true, false,
      true, false, false, true, false, true, true, false, false, true, true, false, true, false, false, true,
      true, false, false, true, false, true, true, false, false, true, true, false, true, false, false, true,
      false, true, true, false, true, false, false, true, true, false, false, true, false, true, true, false,
      false, true, true, false, true, false, false, true, true, false, false, true, false, true, true, false,
      true, false, false, true, false, true, true, false, false, true, true, false, true, false, false, true,
      true, false, false, true, false, true, true, false, false, true, true, false, true, false, false, true,
      false, true, true, false, true, false, false, true, true, false, false, true, false, true, true, false,
      true, false, false, true, false, true, true, false, false, true, true, false, true, false, false, true,
      false, true, true, false, true, false, false, true, true, false, false, true, false, true, true, false,
      false, true, true, false, true, false, false, true, true, false, false, true, false, true, true, false,
      true, false, false, true, false, true, true, false, false, true, true, false, true, false, false, true
    };

    public static bool Parity(byte value) {
      return parityLookup[value];
    }

    // Return the sign of the value (true -> positive)
    public static bool Sign(byte value) {
      return value < 0x80;
    }

    // Return the signed value of a byte in two's complement notation
    public static sbyte Signed(byte value) {
      return (sbyte) (Sign(value) ? value : value - 256); // TODO faster method?
    }

    // Check the state of the n-th bit of a given value.
    public static bool Bit(int value, int n) {
      return (value & 1 << n) > 0;
    }

    // Change the state of the n-th bit of a given value.
    public static byte Bit(byte value, int n, bool state) {
      return state ? SetBit(value, n) : ResetBit(value, n);
    }

    // Set the n-th bit of a given value.
    public static byte SetBit(byte value, int n) {
      return (byte) (value | 1 << n);
    }

    // Reset the n-th bit of a given value.
    public static byte ResetBit(byte value, int n) {
      return (byte) (value & ~(1 << n));
    }

    public static bool IsPrefix(byte opcode) {
      return opcode == 0xED; // TODO handle other tables
    }
  }
}

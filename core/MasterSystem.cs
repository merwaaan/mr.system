using System;
using System.IO;

namespace mr.system {
  public class MasterSystem {

    public readonly CPU cpu;
    public readonly Memory memory;

    public MasterSystem() {
      memory = new Memory();
      cpu = new CPU(memory);
    }

    public void Load(string path) {
      try {
        byte[] rom = File.ReadAllBytes(path);
        memory.Load(rom);
      }
      catch {
        Console.WriteLine($"Cannot open \"{path}\"");
      }
    }

    public void Run() {

    }
  }
}

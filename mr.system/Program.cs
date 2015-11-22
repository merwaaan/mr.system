using System;
using mr.system;

namespace console {
  class Program {
    static void Main(string[] args) {

      MasterSystem ms = new MasterSystem();
      ms.Load("roms/alexkidd.sms");

      string s;
      do {
        ms.cpu.Step();
        s = Console.ReadLine();
      } while (s.Length == 0);
    }
  }
}

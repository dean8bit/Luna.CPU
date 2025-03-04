## Luna.CPU

A modular CPU emulator with customisable asm-like instruction sets.

### Example usage

      var cpu = new CPU(new Memory(32));
      
      cpu.Definitions.AddRange(new IDefinition[] { new LBL(), new ADD(), new SUB(), new JMP(), new SET(), new JLZ() });
      
      cpu.Parse(new string[] {
          "--comment",
          "SET #0 24",
          "SET #2 5",
          "SET #4 1 --comment",
          "--comment",
          "LBL 0",
          "SET >2 #3",
          "ADD >2 #4",
          "SET #3 #4",
          "SET #4 >2",
          "ADD #2 1",
          "SUB #0 1",
          "JLZ #0 1",
          "JMP 0",
          "LBL 1" });
          
      for (int i = 0; i < 201; i++) cpu.Step();
      
      cpu.Memory.GetAt(29, out var value);

---

-- Used as part of a personal game engine. Extracted this module out as an MIT licensed tool.


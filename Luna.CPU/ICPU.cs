namespace Luna.CPU;

public interface ICPU
{
    IMemory Memory { get; set; }
    List<Instruction> Instructions { get; set; }
    StepResult Step();
    ParseResult Parse(string[] code);
    bool SetParameterValue(Parameter parameter, int value);
    bool GetParameterValue(Parameter parameter, out int value);
    void SetPointer(int value);
}

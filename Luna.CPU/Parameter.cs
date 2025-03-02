namespace Luna.CPU;

public enum ParameterType
{
    Constant,
    Location,
    Indirect
}

public enum ParameterConstraint
{
    Constant,
    Memory,
    ConstantOrMemory
}

public class Parameter
{

    public ParameterType Type { get; set; }
    public int Value { get; set; }

    public Parameter(ParameterType type, int value)
    {
        Type = type;
        Value = value;
    }
}

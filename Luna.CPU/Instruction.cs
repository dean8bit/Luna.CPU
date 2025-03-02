namespace Luna.CPU;

public class Instruction
{
    public IDefinition Definition { get; set; }
    public List<Parameter> Parameters = new List<Parameter>();

    public Instruction(IDefinition definition, List<Parameter> parameters)
    {
        Definition = definition;
        Parameters = parameters;
    }
}

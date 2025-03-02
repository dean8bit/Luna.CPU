using System.Text.RegularExpressions;

namespace Luna.CPU;

public class CPU : ICPU
{
    public int Pointer { get; set; }
    public IMemory Memory { get; set; }
    public List<Instruction> Instructions { get; set; } = new List<Instruction>();
    public List<IDefinition> Definitions { get; set; } = new List<IDefinition>();
    public string Commenter { get; set; } = "--";
    public CPU(IMemory memory) => Memory = memory;

    private class DummyDef : IDefinition
    {
        public string Token { get; set; } = "NULL";
        public List<ParameterConstraint> ParameterConstraints { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Func<ICPU, Instruction, bool> Func { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
    private DummyDef DummyDefRef { get; } = new DummyDef();
    private Parameter DummyParameter { get; } = new Parameter(ParameterType.Constant, 0);

    private string ProcessComments(string line)
    {
        var commentIndex = line.IndexOf(Commenter);
        if (commentIndex != -1)
        {
            line = line.Substring(0, commentIndex).TrimEnd();
        }

        return line.Trim();
    }

    private string[] GetInstructionParts(string line)
    {
        return line.Split(' ').Where(v => v != "").ToArray();
    }

    private bool GetDefinition(string token, out IDefinition definition)
    {
        definition = Definitions.Find(v => v.Token == token) ?? DummyDefRef;
        return definition != DummyDefRef;
    }

    public bool SetParameterValue(Parameter parameter, int value)
    {
        switch (parameter.Type)
        {
            case ParameterType.Constant:
                parameter.Value = value;
                return true;
            case ParameterType.Location:
                return Memory.SetAt(parameter.Value, value);
            case ParameterType.Indirect:
                return Memory.SetAtIndirect(parameter.Value, value);
        }
        return false;
    }

    public bool ValidateParameter(Parameter parameter, ParameterConstraint constraint)
    {
        switch (constraint)
        {
            case ParameterConstraint.Constant:
                return parameter.Type == ParameterType.Constant;
            case ParameterConstraint.Memory:
                return parameter.Type != ParameterType.Constant;
            case ParameterConstraint.ConstantOrMemory:
                return true;
            default:
                return false;
        }
    }

    public bool GetParameterValue(Parameter parameter, out int value)
    {
        value = 0;
        switch (parameter.Type)
        {
            case ParameterType.Constant:
                value = parameter.Value;
                return true;
            case ParameterType.Location:
                return Memory.GetAt(parameter.Value, out value);
            case ParameterType.Indirect:
                return Memory.GetAtIndirect(parameter.Value, out value);
        }
        return false;
    }

    public bool GetParameter(string element, out Parameter parameter)
    {
        parameter = DummyParameter;
        var location = element.StartsWith("#");
        var indirect = element.StartsWith(">");
        element = element.Replace("#", "").Replace(">", "");
        if (!Regex.IsMatch(element, @"^-?\d+$")) return false;
        if (!int.TryParse(element, out var value)) return false;

        parameter = new Parameter(location
            ? ParameterType.Location
            : indirect
                ? ParameterType.Indirect
                : ParameterType.Constant, value);
        return true;
    }

    public void SetPointer(int value) => Pointer = value;

    public ParseResult Parse(string[] code)
    {
        Instructions.Clear();

        for (int lineIndex = 0; lineIndex < code.Length; lineIndex++)
        {
            var line = ProcessComments(code[lineIndex]);
            if (line.Length == 0)
            {
                continue;
            }

            var intructionParts = GetInstructionParts(line);
            var token = intructionParts[0];
            var definitionFound = GetDefinition(token, out var definition);
            if (!definitionFound)
            {
                return new ParseResult(false, ParseResultComment.INVALID_INSTRUCTION, lineIndex);
            }

            var parameterCount = definition.ParameterConstraints.Count;
            if (parameterCount != intructionParts.Length - 1)
            {
                return new ParseResult(false, ParseResultComment.INVALID_PARAMETERS, lineIndex);
            }

            var parameters = new List<Parameter>();
            for (var paramIndex = 1; paramIndex <= parameterCount; paramIndex++)
            {
                var foundParameter = GetParameter(intructionParts[paramIndex], out var parameter);
                if (!foundParameter)
                {
                    return new ParseResult(false, ParseResultComment.INVALID_PARAMETER, lineIndex);
                }
                if (!ValidateParameter(parameter, definition.ParameterConstraints[paramIndex - 1]))
                {
                    return new ParseResult(false, ParseResultComment.INVALID_PARAMETER, lineIndex);
                }
                parameters.Add(parameter);
            }
            Instructions.Add(new Instruction(definition, parameters));
        }
        var success = Instructions.Count != 0;
        return new ParseResult(success, success ? ParseResultComment.OK : ParseResultComment.NO_CODE, Instructions.Count);

    }


    public StepResult Step()
    {
        if (Pointer < 0 || Pointer >= Instructions.Count)
        {
            return new StepResult(false, StepResultComment.END_OF_CODE, Pointer);
        }

        var instruction = Instructions[Pointer];
        var result = instruction.Definition.Func(this, instruction);
        if (!result)
        {
            return new StepResult(false, StepResultComment.NO_INSTRUCTION, Pointer);
        }
        Pointer++;
        return new StepResult(true, StepResultComment.OK, Pointer);
    }
}

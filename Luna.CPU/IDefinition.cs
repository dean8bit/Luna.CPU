namespace Luna.CPU;
public interface IDefinition
{
    string Token { get; }
    List<ParameterConstraint> ParameterConstraints { get; }
    Func<ICPU, Instruction, bool> Func { get; }
}

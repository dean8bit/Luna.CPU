namespace Luna.CPU;

public enum ParseResultComment
{
    OK,
    ERROR,
    NO_CODE,
    INVALID_INSTRUCTION,
    INVALID_PARAMETERS,
    INVALID_PARAMETER
}

public enum StepResultComment
{
    OK,
    ERROR,
    END_OF_CODE,
    NO_INSTRUCTION
}

public abstract class Result
{
    public bool Success { get; set; }

    public int Line { get; set; }

    public Result(bool success, int line)
    {
        Success = success;

        Line = line;
    }

}

public class ParseResult : Result
{

    public ParseResultComment Comment { get; set; }
    public ParseResult(bool success, ParseResultComment comment, int line) : base(success, line)
    {
        Comment = comment;
    }
}

public class StepResult : Result
{

    public StepResultComment Comment { get; set; }
    public StepResult(bool success, StepResultComment comment, int line) : base(success, line)
    {
        Comment = comment;
    }
}


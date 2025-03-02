namespace Luna.CPU;

public interface IMemory
{
    bool IsLocationValid(int index);
    bool GetAt(int index, out int value);
    bool SetAt(int index, int value);
    bool GetAtIndirect(int index, out int value);
    bool SetAtIndirect(int index, int value);
    int[] GetAll();
    int GetSize();
    void SetSize(int size);
    void Reset();
}

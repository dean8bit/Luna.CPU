namespace Luna.CPU;
public class Memory : IMemory
{
    private int[] _data = Array.Empty<int>();

    public Memory(int size) => SetSize(size);

    public int[] GetAll() => _data;

    public bool GetAt(int index, out int value)
    {
        value = 0;
        if (!IsLocationValid(index)) return false;
        value = _data[index];
        return true;
    }

    public bool GetAtIndirect(int index, out int value)
    {
        value = 0;
        if (!GetAt(index, out index)) return false;
        if (!GetAt(index, out value)) return false;
        return true;
    }

    public int GetSize() => _data.Length;

    public bool IsLocationValid(int index) => index >= 0 && index < _data.Length;

    public void Reset() => Array.Clear(_data, 0, _data.Length);

    public bool SetAt(int index, int value)
    {
        if (!IsLocationValid(index)) return false;
        _data[index] = value;
        return true;

    }

    public bool SetAtIndirect(int index, int value)
    {
        if (!GetAt(index, out index)) return false;
        if (!SetAt(index, value)) return false;
        return true;
    }

    public void SetSize(int size) => Array.Resize(ref _data, size);
}

public sealed class ExtendedQueue : Queue<string[]>
{
    private readonly int _size;
    public event Action<string[][]> GroupCollected = null!;

    public ExtendedQueue(int size) : base()
    {
        _size = size;

    }

    public new void Enqueue(string[] item)
    {
        base.Enqueue(item);

        if (Count == _size)
        {
            GroupCollected?.Invoke(ToArray());
            Clear();
        }
    }
}
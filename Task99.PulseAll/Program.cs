// при каком сценарии потребовалось бы вызвать PulseAll?

var q = new BoundedQueue<int>(5);

_ = Task.Run(() =>
{
    while (true)
    {
        Thread.Sleep(100);
        var item = q.Dequeue();
        Console.WriteLine($"Item : {item}");
    }
});

Console.ReadKey();
q.Enqueue(42);
Console.ReadKey();

class BoundedQueue<T>
{
    private readonly Queue<T> _queue = new();
    private readonly int _maxSize;
    private readonly object _lock = new();

    public BoundedQueue(int maxSize)
    {
        _maxSize = maxSize;
    }

    public void Enqueue(T item)
    {
        lock (_lock)
        {
            while (_queue.Count >= _maxSize)
            {
                Monitor.Wait(_lock);
            }

            _queue.Enqueue(item);
            Monitor.Pulse(_lock);
        }
    }

    public T Dequeue()
    {
        lock (_lock)
        {
            while (_queue.Count == 0)
            {
                Monitor.Wait(_lock);
            }

            var item = _queue.Dequeue();
            Monitor.Pulse(_lock);

            return item;
        }
    }
}

// при каком сценарии потребовалось бы вызвать PulseAll?


var N = 0;

var q = new BoundedQueue<int>(5);

Enumerable.Range(1, 3).Select(i => Task.Run(() =>
{
    while (true)
    {
        Thread.Sleep(100);
        try
        {
            var item = q.Dequeue();
            Console.WriteLine($"{i} << : {item}");
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine($"{i} dequeue canceled");
            return;
        }
    }
})).ToList();


Enumerable.Range(1, 2).Select(i => Task.Run(() =>
{
    while (true)
    {
        Thread.Sleep(300);

        var item = Interlocked.Increment(ref N);

        try
        {
            q.Enqueue(item);
            Console.WriteLine($"{i} >> : {item}");
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine($"{i} enqueue canceled");
            return;
        }        
    }   

})).ToList();


Thread.Sleep(1000);
q.Cancel();
Thread.Sleep(1000);


class BoundedQueue<T>
{
    private readonly Queue<T> _queue = new();
    private readonly int _maxSize;
    private readonly object _lock = new();
    private bool _cancelled = false;

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
                if (_cancelled)
                {
                    throw new OperationCanceledException();
                }
                Monitor.Wait(_lock);
            }

            if (_cancelled)
            {
                throw new OperationCanceledException();
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
                {
                    if (_cancelled)
                    {
                        throw new OperationCanceledException();
                    }
                }
                Monitor.Wait(_lock);
            }

            var item = _queue.Dequeue();
            Monitor.Pulse(_lock);

            return item;
        }
    }

    public void Cancel()
    {
        lock (_lock)
        {
            _cancelled = true;
            Monitor.PulseAll(_lock);
        }
    }
}

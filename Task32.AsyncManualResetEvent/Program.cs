var mre = new MyManualResetEvent(false);

var tasks = Enumerable.Range(1, 4).Select(WorkerAsync).ToList();


await Task.Delay(1000);

Console.WriteLine(".....................Set");
mre.Set();
mre.Set();

mre.Reset();

await Task.Delay(1000);

Console.WriteLine(".....................Set");
mre.Set();



await Task.WhenAll(tasks);

async Task WorkerAsync(int i)
{
    Console.WriteLine($"{i} waiting {Environment.CurrentManagedThreadId}");

    await mre.WaitOne();
    Console.WriteLine($"{i} processed, phase A, {Environment.CurrentManagedThreadId}");


    await Task.Delay(1000);

    await mre.WaitOne();
    Console.WriteLine($"{i} processed, phase B, {Environment.CurrentManagedThreadId}");


    await Task.Delay(1000);
}


class MyManualResetEvent
{
    public MyManualResetEvent(bool initialState)
    {
        if (initialState)
        {
            _tcs.SetResult();
        }
    }

    public void Set()
    {
        var tcs = _tcs;

        // как без проверки результата сообщать результат несколько раз.

        _ = tcs.TrySetResult();
    }

    public void Reset()
    {
        Interlocked.Exchange(ref _tcs, new());
    }

    public async Task WaitOne()
    {
        var tcs = _tcs;

        await tcs.Task;
      
        while (Interlocked.Exchange(ref tcs, _tcs) != _tcs)
        {            
            await tcs.Task;
        }
    }

    private TaskCompletionSource _tcs = new();
}


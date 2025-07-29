using System.Diagnostics;

var barrier = new AsyncBarrier(5);

var random = new Random();

Enumerable.Range(1, 5).Select(i => Task.Run(async () =>
{
    foreach (var phase in "ABCDEF")
    {
        Thread.Sleep(random.Next(300, 1300));
        Console.WriteLine($"{i} completed {phase}");
        await barrier.SignalAndWaitAsync();

        Console.WriteLine($"--------");
    }

})).ToList();


await Task.Delay(100000);

class AsyncBarrier(int _participantCount)
{
    public Task SignalAndWaitAsync()
    {
        var tcs = _tcs;
        
        if (Interlocked.Increment(ref _currentCount) == _participantCount)
        {
            Interlocked.Exchange(ref _currentCount, 0);
            _tcs = new(TaskCreationOptions.RunContinuationsAsynchronously);
            tcs.SetResult();
        }

        return tcs.Task;
    }

    TaskCompletionSource _tcs = new(TaskCreationOptions.RunContinuationsAsynchronously);
    int _currentCount = 0;
}
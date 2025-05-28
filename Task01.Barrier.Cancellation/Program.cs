StopperToken token = new();

Console.WriteLine("Main: letting worker run for 5 sec");
var waiter = Worker(); ;
await Task.Delay(5000);
token.IsStopped = true;
Console.WriteLine("Main: waiting for worker to stop");
await waiter;

async Task Worker()
{
    await Task.Yield();
    int x = 0;

    while (!token.IsStopped)
    {
        x = UpdateItem(x);
    }

    Console.WriteLine($"Worker: stopped");
}

int UpdateItem(int item)
{
    return item;
}

public struct StopperToken
{
    public volatile bool IsStopped;
}
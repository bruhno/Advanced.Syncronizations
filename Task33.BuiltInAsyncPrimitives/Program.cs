var s = new SemaphoreSlim(3);

Enumerable.Range(1, 9).Select(i => Task.Run(async () =>
{
    await s.WaitAsync();
    await Task.Delay(2000);
    Console.WriteLine($"{i} done");
    s.Release();
})).ToList();


await Task.Delay(10000);
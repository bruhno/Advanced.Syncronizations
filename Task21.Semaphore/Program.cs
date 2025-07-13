// Ограничение одновременно работающих обработчиков

//var s = new Semaphore(3, 3);
var s = new SemaphoreSlim(3, 3);

Parallel.For(1, 30, Handle);

void Handle(int num)
{
    //s.WaitOne();
    s.Wait();
    Thread.Sleep(1000);
    s.Release();
    Console.WriteLine($"{num} done");
}
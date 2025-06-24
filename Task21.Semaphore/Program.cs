// Ограничение одновременно работающих обработчиков

var s = new Semaphore(3, 3);

Parallel.For(1, 30, Handle);

void Handle(int num)
{
    s.WaitOne();
    Thread.Sleep(1000);
    s.Release();
    Console.WriteLine($"{num} done");
}
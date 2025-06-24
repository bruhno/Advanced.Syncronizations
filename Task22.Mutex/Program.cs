// Эксклюзивный доступ к ресурсу
var m = new Mutex();

Parallel.For(0, 20, Handle);

void Handle(int num)
{
    m.WaitOne();
    Thread.Sleep(1000);
    Console.WriteLine($"{num} done");
    m.ReleaseMutex();
}
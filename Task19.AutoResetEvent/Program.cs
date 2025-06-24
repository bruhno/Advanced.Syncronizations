// Когда мы хотим чтобы работы выполнялись по очереди.
var re = new AutoResetEvent(true);

Parallel.For(0, 20, Handle);

void Handle(int num)
{
    re.WaitOne();
    Thread.Sleep(100);
    Console.WriteLine($"{num} done");
    re.Set();
}

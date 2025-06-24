// Ожидание события множеством обработчиков
var re = new ManualResetEvent(false);

_ = Task.Run(Prepare);

Parallel.For(1, 20, Handle);

void Prepare()
{
    Thread.Sleep(1000);
    Console.WriteLine("preparing complete");
    re.Set();
}

void Handle(int num)
{
    re.WaitOne();
    Thread.Sleep(200);
    Console.WriteLine($"{num} done");
}
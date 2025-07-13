// Ожидание события множеством обработчиков
//var re = new ManualResetEvent(false);
var re = new ManualResetEventSlim(false);

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
    //re.WaitOne();
    re.Wait();
    Thread.Sleep(200);
    Console.WriteLine($"{num} done");
}
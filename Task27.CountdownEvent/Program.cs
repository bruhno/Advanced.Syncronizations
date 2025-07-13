// Нужно дождаться завершения
// определенного количества операций

var N = 4;

var cde = new CountdownEvent(N);

Parallel.For(0, N, Work);

cde.Wait();

Console.WriteLine("All complete");

void Work(int num)
{
    Thread.Sleep(500 * num);
    var r = cde.Signal();
    Console.WriteLine($"{num} done,  i'm last {r}");
}
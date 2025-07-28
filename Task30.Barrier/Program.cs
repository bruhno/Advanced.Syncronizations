var N = 4;

var random = new Random();

var arr = new int[N];

var barrier = new Barrier(N, b =>
{
    var phase = b.CurrentPhaseNumber+1;

    var avg = (int)arr.Average() / phase;
    var max = arr.Max() / phase;

    Console.WriteLine($"After {phase} average = {avg}, max = {max}");
});

var tasks = Enumerable.Range(1, 4).Select(i => Task.Run(() => Work(i))).ToList();

Task.WaitAll(tasks);

void Work(int num)
{
    for (var i = 0; i < 100; i++)
    {
        arr[num-1] += random.Next(100, 300);

        Thread.Sleep(random.Next(100, 300));

        barrier.SignalAndWait();
    }
}
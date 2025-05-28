bool s_stopWorker = false;

Console.WriteLine("Main: letting worker run for 5 sec");
Thread t = new Thread(Worker);
t.Start();
Thread.Sleep(5000);
s_stopWorker = true;
Console.WriteLine("Main: waiting for worker to stop");
t.Join();


void Worker(object o)
{
    int x = 0;

    // Problem: It can be locked in the Release mode
    while (!s_stopWorker)
    {
        x++;
    }

    // Solution 1: Use Volatile.Read to ensure visibility of the stop signal
    //while (!Volatile.Read(ref s_stopWorker))
    //{
    //    x++;
    //}

    // Solution 2: Use Thread.MemoryBarrier to ensure visibility of the stop signal
    //while (!s_stopWorker)
    //{
    //    Thread.MemoryBarrier();
    //    x++;
    //}

    Console.WriteLine($"Worker: stopped when x ={x}");
}


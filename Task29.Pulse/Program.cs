using System.Diagnostics;

var random = new Random();

var sw = Stopwatch.StartNew();

var _lock = new object();

var text = "The old lady pulled her spectacles down and looked over them about the room; then she put them up and looked out under them. She seldom or never looked";

var data = string.Empty;

var dataReady = false;

var waiterReady = false;

var done = false;

Enumerable.Range(1, 5).Select(i => Task.Run(() => Waiter(i))).ToList();

lock (_lock)
{
    while (!waiterReady)
    {
        Monitor.Wait(_lock);
    }
}

foreach (var word in text.Split(" "))
{
    lock (_lock)
    {       
        while (dataReady)
        {
            Monitor.Wait(_lock);
        }

        Thread.Sleep(100);

        data = word;

        dataReady = true;        

        Monitor.Pulse(_lock);
        Console.WriteLine($".................... pulse: {word}");
    }
    Thread.Sleep(100);
}

lock (_lock)
{
    done = true;
    Monitor.PulseAll(_lock);
}

void Waiter(int num)
{
    while (true)
    {
        lock (_lock)
        {
            while (!dataReady)
            {
                if (done) return;

                if (!waiterReady)
                {
                    waiterReady = true;
                    Monitor.PulseAll(_lock);                    
                }                
                Monitor.Wait(_lock);                
            }

            Thread.Sleep(300);
            dataReady = false;            

            Console.WriteLine($"{num}: {data}");
        }
        Thread.Sleep(random.Next(100,300));
    }
}
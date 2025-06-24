bool running = true;

// Фоновый поток для создания конкуренции на CPU
for (int i = 0; i < Environment.ProcessorCount; i++)
{
    new Thread(() => {
        while (running) { double x = Math.Sqrt(DateTime.Now.Ticks); }
    }).Start();
}


Thread thread1 = new Thread(() => Run("Sleep(1)", () => Thread.Sleep(1)));
Thread thread2 = new Thread(() => Run("Sleep(0)", () => Thread.Sleep(0)));
Thread thread3 = new Thread(() => Run("Yield", () => Thread.Yield()));

thread1.Start();
thread2.Start();
thread3.Start();

// Пусть работает 2 секунды
Thread.Sleep(2000);
Volatile.Write(ref running, false);

thread1.Join();
thread2.Join();
thread3.Join();

void Run(string label, Action yieldAction)
{
    int count = 0;
    while (running)
    {
        count++;
        yieldAction(); // Уступка
    }
    Console.WriteLine($"{label,-10}: {count} iterations");
}
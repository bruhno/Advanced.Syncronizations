// Семантика чтения-записи
// Читатели блокируют писателей
// Читатели не блокируют других читателей
// Писатели блокируют и читателей, и писателей


var rw = new ReaderWriterLock();


Enumerable.Range(1, 10).Select(_ => Task.Run(Read)).ToList();

Enumerable.Range(1, 2).Select(_ => Task.Run(Write)).ToList();


await Task.Delay(10_000);

void Read()
{
    var cnt = 1;
    while (true)
    {
        rw.AcquireReaderLock(1000);
        Thread.Sleep(500);
        Console.WriteLine($"{Environment.CurrentManagedThreadId} read {cnt++} times");
        rw.ReleaseReaderLock();

        Thread.Sleep(200);
    }
}

void Write()
{
    var cnt = 1;
    while (true)
    {
        rw.AcquireWriterLock(10000);
        Thread.Sleep(1000);
        Console.WriteLine($"                   {Environment.CurrentManagedThreadId} write {cnt++} times");
        rw.ReleaseWriterLock();

        Thread.Sleep(1000);
    }
}
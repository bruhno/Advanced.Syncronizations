using System;
using System.Diagnostics;

var rw = new ReaderWriterLockSlim();

var sw = Stopwatch.StartNew();

//Апгрейд, второй поток ждет окончания чтения

_ = Task.Run(() =>
{
    rw.EnterUpgradeableReadLock();
    Console.WriteLine($"{sw.ElapsedMilliseconds}: {Environment.CurrentManagedThreadId} read started");

    Thread.Sleep(500);

    rw.EnterWriteLock();
    Console.WriteLine($"{sw.ElapsedMilliseconds}: {Environment.CurrentManagedThreadId} upgraded to write");

    Thread.Sleep(1000);

    rw.ExitWriteLock();
    //rw.DowngradeFromWriterLock(ref cookie);
    Console.WriteLine($"{sw.ElapsedMilliseconds}: {Environment.CurrentManagedThreadId} downgraded to read");

    Thread.Sleep(1000);

    rw.ExitUpgradeableReadLock();
    //rw.ReleaseReaderLock();
    Console.WriteLine($"{sw.ElapsedMilliseconds}: {Environment.CurrentManagedThreadId} released");
});


_ = Task.Run(() =>
{
    //rw.AcquireReaderLock(10000);
    rw.EnterReadLock();
    Console.WriteLine($"{sw.ElapsedMilliseconds}: {Environment.CurrentManagedThreadId} read started");

    Thread.Sleep(1000);

    //rw.ReleaseReaderLock();
    rw.ExitReadLock();
    Console.WriteLine($"{sw.ElapsedMilliseconds}: {Environment.CurrentManagedThreadId} released");
});

_ = Task.Run(() =>
{
    Thread.Sleep(1500);

    //rw.AcquireWriterLock(10000);
    rw.EnterWriteLock();
    Console.WriteLine($"{sw.ElapsedMilliseconds}: {Environment.CurrentManagedThreadId} write started");

    Thread.Sleep(1000);

    //rw.ReleaseWriterLock();
    rw.ExitWriteLock();
    Console.WriteLine($"{sw.ElapsedMilliseconds}: {Environment.CurrentManagedThreadId} released");
});

await Task.Delay(10000);

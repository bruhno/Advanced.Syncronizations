var timeout = 10000;

var rw = new ReaderWriterLock();


_ = Task.Run(() =>
{
    Console.WriteLine("try to read");
    
    rw.AcquireReaderLock(timeout);

    Console.WriteLine("reading");

    rw.AcquireWriterLock(timeout);

    //var al = rw.UpgradeToWriterLock(timeout);

    Console.WriteLine("writing");

    

    Console.WriteLine("reading again");

    rw.ReleaseReaderLock();

    Console.WriteLine("done");
});


await Task.Delay(10000);
var i = 0;

var spinLock = new SpinLock(false);

Parallel.For(0, 100, Increment);

Console.WriteLine(i);

#region demo of enableThreadOwnerTracking
var taken = false;
_ = Task.Run(() => spinLock.Enter(ref taken));
Thread.Sleep(100);
spinLock.Exit();
#endregion



void Increment(int _)
{
    for (var j = 0; j < 100; j++)
    {
        var taken = false;

        spinLock.Enter(ref taken);
        if (taken)
        {
            i++;
            spinLock.Exit();
        }
        else
        {
            Console.WriteLine("not taken");
        }
    }
}


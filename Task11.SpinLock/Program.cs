var sum = 0;

var myLock = new MySpinLock();

Parallel.For(0, 10, _ => Increment());

Console.WriteLine(sum);

void Increment()
{
    for (var i = 0; i < 100000; i++)
    {
        myLock.Enter();
        sum++;
        myLock.Exit();
    }
}

class MySpinLock
{
    public void Enter()
    {
        while (Interlocked.Exchange(ref _locked, true)) ;
    }

    public void Exit()
    {
        Interlocked.Exchange(ref _locked, false);
    }

    private bool _locked;
}
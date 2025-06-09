var value = -1;
var initialized = false;

Parallel.For(1, 1001, Init);

Console.WriteLine($"Final value: {value}");

void Init(int x)
{
    var newValue = CalcValue(x);   
    
    if (!Interlocked.Exchange(ref initialized, true))
    {
        value = newValue;
        Console.WriteLine($"Initialized: {newValue}");
    }
}

int CalcValue(int x)
{
    Thread.Sleep(50);
    return x;
}
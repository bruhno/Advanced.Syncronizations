var value = -1;

Parallel.For(1, 101, Init);

Console.WriteLine($"Final value: {value}");

void Init(int x)
{

    var newValue = CalcValue(x);

    var original = Interlocked.Exchange(ref value, newValue);

    if (original == -1)
    {
        Console.WriteLine($"Initialized: {newValue}");
        return;
    };

    while (original != Interlocked.Exchange(ref value, original)) { }

    //Console.WriteLine($"Initialized: {newValue}, original {original}");

}

int CalcValue(int x)
{
    Thread.Sleep(10);
    return x;
}
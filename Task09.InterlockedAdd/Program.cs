var sum = 0;


Parallel.For(0, 100, _ => Increment());

Console.WriteLine(sum);

void Increment()
{
    for (var i = 0; i < 100; i++)
    {
        Interlocked.Add(ref sum, 1);
        //sum = sum + 1;
    }
}

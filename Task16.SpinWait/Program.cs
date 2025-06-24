

var value = false;

_ = Task.Run(async () =>
{
    await Task.Delay(1000);
    value = true;
});

var sw = new SpinWait();
while (!value)
{
    if (sw.NextSpinWillYield)
    {
        Console.WriteLine($"next yield:{sw.Count}");
    }
    sw.SpinOnce();
}
Console.WriteLine($"spin count: {sw.Count}");


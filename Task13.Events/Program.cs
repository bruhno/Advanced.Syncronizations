
var source = new Source();

var counter = 0;

Parallel.For(0, 100, _ =>
    source.AddHandler(() => Interlocked.Increment(ref counter)));

source.Run();


Console.WriteLine($"counter: {counter}");

class Source
{
    public void AddHandlerUnsafe(Action action)
    {
        _event += action;
    }

    public void AddHandler(Action action)
    {
        while(true)
        {
            var oldEvent = _event;
            var newEvent = _event + action;

            if (Interlocked.CompareExchange(ref _event, newEvent, oldEvent) == oldEvent)
            {
                return;
            }            
        } 
    }

    public void Run()
    {
        _event?.Invoke();
    }

    private Action? _event;
}
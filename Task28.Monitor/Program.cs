var a = new Lock();

using var c = a.EnterScope();

using var b = a.EnterScope();

var t = new Thread(_ =>
{
    Console.WriteLine("Thread Try");
    using var d = a.EnterScope();
    Console.WriteLine("Thread Entered");
});


t.Start();

t.Join();

Console.WriteLine("ssssssss");
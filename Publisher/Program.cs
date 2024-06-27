// See https://aka.ms/new-console-template for more information

using Nats_Test;

Console.WriteLine("Hello, World! I am Publisher");

Console.WriteLine("Select Publish strategy:" +
    "\n 1 = Publish to single subject" +
    "\n 2 = publish to separate subjects" +
    "\n 3 = publish to single subject with one task");

ConsoleKeyInfo x = Console.ReadKey();

NatsPublisher publisher = new NatsPublisher();
if (x.Key == ConsoleKey.D1)
{
    publisher.PublishToSingleSubject();
}
if (x.Key == ConsoleKey.D2)
{
    publisher.PublishToMultipleSubject();
}
if (x.Key == ConsoleKey.D3)
{
    publisher.PublishToSingleSubjectWithOneTask();
}



Console.ReadKey();

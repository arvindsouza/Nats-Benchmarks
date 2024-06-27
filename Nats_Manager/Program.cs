// See https://aka.ms/new-console-template for more information
using Nats_Manager;

Console.WriteLine("Hello, World! I am Manager");
Speaker speaker = new Speaker();

Console.WriteLine("How do you wish me to speak?" +
    "\n 1 = One recipient - Work Queue" +
    "\n 2 = Multiple recipients - Interest Queue");

ConsoleKeyInfo x = Console.ReadKey();

if (x.Key == ConsoleKey.D1)
{
    speaker.CreateWorkQueueStream();
}
else
    if (x.Key == ConsoleKey.D2)
{
    speaker.CreateInterestStream();
}
Console.WriteLine("Created Stream");


Console.ReadLine();

speaker.Dispose();


// See https://aka.ms/new-console-template for more information
using Nats_Subscriber;

Console.WriteLine("Hello, World! I am subscriber");

NatsSubscriber subscriber = new NatsSubscriber();

Console.WriteLine("Select subscription strategy:" +
    "\n 1 = subscribe with single consumer on one subjects" +
    "\n 2 = subscribe with multiple consumers on one subject" +
    "\n 3 = subscribe with multiple consumers with unique subject for each");



var x = Console.ReadKey();

if (x.Key == ConsoleKey.D1)
{
    subscriber.SubscibeSingleConsumer();
}
else
if (x.Key == ConsoleKey.D2)
{
    subscriber.SubscribeMultipleConsumersOneSubject();

}
else
if (x.Key == ConsoleKey.D3)
{
    subscriber.SubscribeMultipleConsumersManySubject();

}


Console.WriteLine("\n Subscribed");
Console.ReadKey();

//subscriber.Dispose();

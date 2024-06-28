using NATS.Client.JetStream;


namespace Nats_Subscriber
{
    public  class Receptacle
    {
        public string ConsumerName { get; set; }

        public NatsJSMsg<byte[]> Message { get; set; }
    }
}

using Commons.BusinessModels;
using NATS.Client.JetStream;


namespace Nats_Subscriber
{
    public  class Receptacle<T>
    {
        public string ConsumerName { get; set; }

        public NatsJSMsg<T> Message { get; set; }
    }

}

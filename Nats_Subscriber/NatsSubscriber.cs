using NATS.Client.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NATS.Client;
using System.ComponentModel;
using Commons;
using Google.Protobuf;
using Commons.BusinessModels;
using System.Collections.Concurrent;
using System.Diagnostics;
using NATS.Client.JetStream.Models;
using System.IO;
using NATS.Client.JetStream;
using Serilog;
using Serilog.Core;
using System.Diagnostics.Metrics;
using System.Threading.Channels;

namespace Nats_Subscriber
{
    public class NatsSubscriber
    {

        EventHandler<HandledEventArgs> mIncomingFrameHandler = NatsReceiveImageEventHandler;
        AutoResetEvent mMainThreadEvent = new AutoResetEvent(false);
        INatsJSStream mStream;
        NatsJSContext mJetstream;
        // List<INatsJSConsumer> mConsumers = new List<INatsJSConsumer>();

        // Logger SubscriberLogger = new LoggerConfiguration().WriteTo.File("Subscriber.log").CreateLogger();
        Logger SubscriberLogger = new LoggerConfiguration()
            //  .WriteTo.Console()
            .WriteTo.File("logs\\SubscriberLog.log").CreateLogger();

        static void NatsReceiveImageEventHandler(object? sender, HandledEventArgs args)
        {
            Console.WriteLine("Received");
        }

        ConcurrentQueue<Receptacle<TransportUnit2>> MessageQueue = new ConcurrentQueue<Receptacle<TransportUnit2>>();
        public NatsSubscriber()
        {
            this.GetStream();
            this.StartReader();
        }

        public async void GetStream()
        {
            try
            {
                NatsConnection nats = new NatsConnection(new NatsOpts
                {
                    SubPendingChannelFullMode = BoundedChannelFullMode.Wait,
                    SerializerRegistry = new MyProtoBufSerializerRegistry()
                });
                this.mJetstream = new NatsJSContext(nats);
                mStream = await mJetstream.GetStreamAsync(StreamDetails.STREAM_NAME);
                SubscriberLogger.Information($"Initialized Subscriber");

            }
            catch (Exception ex)
            {

            }
        }


        public void StartReader()
        {
            Task.Run(() =>
            {
                int counter = 0;
                Stopwatch st = new Stopwatch();
                st.Start();
                while (true)
                {
                    Receptacle<TransportUnit2> dequeuedMessage = new Receptacle<TransportUnit2>();

                    if (this.MessageQueue.Count == 0)
                        this.mMainThreadEvent.WaitOne();

                    this.MessageQueue.TryDequeue(out dequeuedMessage);

                    if (dequeuedMessage != null && dequeuedMessage.Message.Data != null)
                    {
                        TransportUnit2 message = dequeuedMessage.Message.Data;
                        SubscriberLogger.Information($"Deserialized message {message.DatapointKey} from consumer {dequeuedMessage.ConsumerName}");

                        counter++;
                        if (counter >= StreamDetails.NUMBER_OF_TASKS * StreamDetails.TOTAL_MESSAGES_PER_TASK)
                        {
                            st.Stop();
                            SubscriberLogger.Information($"Processed all messages {counter} in {st.ElapsedMilliseconds}");
                            Console.WriteLine($"Processed all messages {counter} in {st.ElapsedMilliseconds}");

                        }
                    }
                }


            });

        }
        public async void SubscibeSingleConsumer()
        {
            NatsConnection nats = new NatsConnection();
            var cts = new CancellationTokenSource();
            int i = 0, j = 0, k = 0, l = 0, m = 0, n = 0, q = 0, r = 0, x = 0;

            string streamName = "test";
            string streamName2 = "TEST";


            //  jetstream.DeleteStreamAsync(streamName2);
            var consumer = await mStream.CreateOrUpdateConsumerAsync(new ConsumerConfig("processor-1"));

            //            var sub = nats.SubscribeAsync<byte[]>(subject: "picture");
            var subscription = Task.Run(async () =>
            {
                await foreach (var msg in consumer.ConsumeAsync<TransportUnit2>(opts: new NatsJSConsumeOpts { MaxMsgs = StreamDetails.MAX_CONSUMER_MESSAGES }))
                {
                    this.MessageQueue.Enqueue(this.GetMessage(consumer.Info.Name, msg));
                    SubscriberLogger.Information($"Enqueued, Total: {x++}");
                    this.mMainThreadEvent.Set();
                    await msg.AckAsync();


                    // Console.WriteLine($"{x++}");
                }
            });
        }

        public async void SubscribeMultipleConsumersOneSubject()
        {
            List<INatsJSConsumer> consumers = new List<INatsJSConsumer>();

            for (int i = 0; i < StreamDetails.NUMBER_OF_TASKS; i++)
            {
                consumers.Add(await mStream.CreateOrUpdateConsumerAsync(new ConsumerConfig($"processor-{i + 1}")
                {
                    AckPolicy = ConsumerConfigAckPolicy.Explicit
                }));
                SubscriberLogger.Information($"Created consumer {consumers[i].Info.Name}");

            }

            foreach (INatsJSConsumer consumer in consumers)
            {
                this.CreateConsumerTask(consumer);
            }
            //            var sub = nats.SubscribeAsync<byte[]>(subject: "picture");
        }

        public async void SubscribeMultipleConsumersManySubject()
        {
            List<INatsJSConsumer> consumers = new List<INatsJSConsumer>();

            for (int i = 0; i < StreamDetails.NUMBER_OF_TASKS; i++)
            {
                consumers.Add(await mStream.CreateOrUpdateConsumerAsync(new ConsumerConfig($"processor-{i + 1}")
                {
                    FilterSubject = $"{StreamDetails.SUBJECT_NAME}.picture{i}",
                    AckPolicy = ConsumerConfigAckPolicy.Explicit,
                }));
                SubscriberLogger.Information($"Created consumer {consumers[i].Info.Name}");

            }

            foreach (INatsJSConsumer consumer in consumers)
            {
                this.CreateConsumerTask(consumer);
            }
            //            var sub = nats.SubscribeAsync<byte[]>(subject: "picture");
        }

        public void CreateConsumerTask(INatsJSConsumer consumer)
        {
            Task.Run(async () =>
            {
                int counter = 0;
                string consumerName = consumer.Info.Name;
                await foreach (var msg in consumer.ConsumeAsync<TransportUnit2>(opts: new NatsJSConsumeOpts { MaxMsgs = StreamDetails.MAX_CONSUMER_MESSAGES }))
                {
                    await msg.AckAsync();
                    this.MessageQueue.Enqueue(GetMessage(consumerName, msg));
                    // SubscriberLogger.Information($"Enqueued, Total: {counter++} for consumer {consumer.Info.Name}");
                    this.mMainThreadEvent.Set();


                    // Console.WriteLine($"{x++}");
                }
            });
        }
            //            var sub = nats.SubscribeAsync<byte[]>(subject: "picture");
        public Receptacle<TransportUnit2> GetMessage(string consumerName, object message)
        {
            Receptacle<TransportUnit2> result = new Receptacle<TransportUnit2>();

            result.ConsumerName = consumerName;
            result.Message = (NatsJSMsg<TransportUnit2>) message;


            return result;
        }
    }
}

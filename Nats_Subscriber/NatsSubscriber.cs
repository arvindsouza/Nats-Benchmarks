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

namespace Nats_Subscriber
{
    public class NatsSubscriber
    {

        EventHandler<HandledEventArgs> mIncomingFrameHandler = NatsReceiveImageEventHandler;
        AutoResetEvent mMainThreadEvent = new AutoResetEvent(false);
        INatsJSStream mStream;
        NatsJSContext mJetstream;

        static void NatsReceiveImageEventHandler(object? sender, HandledEventArgs args)
        {
            Console.WriteLine("Received");
        }

        ConcurrentQueue<NatsJSMsg<byte[]>> MessageQueue = new ConcurrentQueue<NatsJSMsg<byte[]>>();
        public NatsSubscriber()
        {
            this.GetStream();
            this.StartReader();
        }

        public async void GetStream()
        {
            try
            {
                NatsConnection nats = new NatsConnection();
                this.mJetstream = new NatsJSContext(nats);
                mStream = await mJetstream.GetStreamAsync(StreamDetails.STREAM_NAME);
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
                    NatsJSMsg<byte[]> dequeuedMessage = new NatsJSMsg<byte[]>();

                    if (this.MessageQueue.Count == 0)
                        this.mMainThreadEvent.WaitOne();

                    this.MessageQueue.TryDequeue(out dequeuedMessage);
                    //  Console.WriteLine($"Queue size: {this.MessageQueue.Count}");

                    //if(dequeuedMessage.Data == null)
                    //{
                    //    Console.WriteLine("Null data");
                    //}

                    if (dequeuedMessage.Data != null)
                    {
                        TransportUnit message = ProtoHelper.DeserializeDecompressFromBytes(dequeuedMessage.Data);
                        counter++;
                        if(counter == StreamDetails.NUMBER_OF_TASKS * StreamDetails.TOTAL_MESSAGES_PER_TASK)
                        {
                             st.Stop();
                             Console.WriteLine($"Processed all messagees {counter} in {st.ElapsedMilliseconds}");

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
                await foreach (var msg in consumer.ConsumeAsync<byte[]>(opts: new NatsJSConsumeOpts { MaxBytes = 400000000 }))
                {
                    this.MessageQueue.Enqueue(msg);
                    Console.WriteLine($"Enqueued, Total: {x++}");
                    this.mMainThreadEvent.Set();
                    await msg.AckAsync();


                    // Console.WriteLine($"{x++}");
                }
            });
        }

        public async void SubscribeMultipleConsumersOneSubject()
        {
            for(int i=0;i<StreamDetails.NUMBER_OF_TASKS;i++)
            {
                var consumer = await mStream.CreateOrUpdateConsumerAsync(new ConsumerConfig($"processor-{i+1}"));

                Task.Run(async () =>
                {
                    int counter = 0;
                    Console.WriteLine($"Created consumer {consumer.Info.Name}");

                    await foreach (var msg in consumer.ConsumeAsync<byte[]>(opts: new NatsJSConsumeOpts { MaxBytes = 400000000 }))
                    {
                        this.MessageQueue.Enqueue(msg);
                        Console.WriteLine($"Enqueued, Total: {counter++} for consumer {consumer.Info.Name}");
                        this.mMainThreadEvent.Set();
                        await msg.AckAsync();


                        // Console.WriteLine($"{x++}");
                    }
                });
            }
            //            var sub = nats.SubscribeAsync<byte[]>(subject: "picture");
        }
    }
}

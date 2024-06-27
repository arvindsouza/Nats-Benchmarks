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
        static void NatsReceiveImageEventHandler(object? sender, HandledEventArgs args)
        {
            Console.WriteLine("Received");
        }

        ConcurrentQueue<NatsJSMsg<byte[]>> MessageQueue = new ConcurrentQueue<NatsJSMsg<byte[]>>();
        public NatsSubscriber()
        {
            this.Subscibe();
        }


        async void Subscibe()
        {
            Stopwatch st = new Stopwatch();

            NatsConnection nats = new NatsConnection();
            var cts = new CancellationTokenSource();
            int i = 0, j = 0, k = 0, l = 0, m = 0, n = 0, q = 0, r = 0, x = 0;

            var jetstream = new NatsJSContext(nats);
            string streamName = "test";
            string streamName2 = "TEST";

            Task.Run(() =>
            {

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
                        // st.Stop();
                        // Console.WriteLine($"Deserialization time {st.ElapsedMilliseconds}");

                        if (message.DatapointKey == "1")
                        {
                            i++;
                            Console.WriteLine($"Received data {message.DatapointKey} @ {message.DatapointName}, Total: {i}");

                            if (i == 100000)
                                Console.WriteLine($"Got all {message.DatapointKey} data in {st.ElapsedMilliseconds}ms");
                        }
                        if (message.DatapointKey == "2")
                        {
                            j++;
                            Console.WriteLine($"Received data {message.DatapointKey} @ {message.DatapointName}, Total: {j}");

                            if (j == 100000)
                                Console.WriteLine($"Got all {message.DatapointKey} data in {st.ElapsedMilliseconds}ms");
                        }
                        if (message.DatapointKey == "3")
                        {
                            k++;
                            Console.WriteLine($"Received data {message.DatapointKey} @ {message.DatapointName}, Total: {k}");

                            if (k == 100000)
                                Console.WriteLine($"Got all {message.DatapointKey} data in {st.ElapsedMilliseconds}ms");
                        }
                        if (message.DatapointKey == "4")
                        {
                            l++;
                            Console.WriteLine($"Received data {message.DatapointKey} @ {message.DatapointName}, Total: {l}");

                            if (l == 100000)
                                Console.WriteLine($"Got all {message.DatapointKey} data in {st.ElapsedMilliseconds}ms");
                        }
                        if (message.DatapointKey == "5")
                        {
                            m++;
                            Console.WriteLine($"Received data {message.DatapointKey} @ {message.DatapointName}, Total: {m}");

                            if (m == 100000)
                                Console.WriteLine($"Got all {message.DatapointKey} data in {st.ElapsedMilliseconds}ms");
                        }
                        if (message.DatapointKey == "6")
                        {
                            n++;
                            Console.WriteLine($"Received data {message.DatapointKey} @ {message.DatapointName}, Total: {n}");

                            if (n == 100000)
                                Console.WriteLine($"Got all {message.DatapointKey} data in {st.ElapsedMilliseconds}ms");
                        }
                        if (message.DatapointKey == "7")
                        {
                            q++;
                            Console.WriteLine($"Received data {message.DatapointKey} @ {message.DatapointName}, Total: {q}");

                            if (q == 100000)
                                Console.WriteLine($"Got all {message.DatapointKey} data in {st.ElapsedMilliseconds}ms");
                        }
                        if (message.DatapointKey == "8")
                        {
                            r++;
                            Console.WriteLine($"Received data {message.DatapointKey} @ {message.DatapointName}, Total: {r}");

                            if (r == 100000)
                                Console.WriteLine($"Got all {message.DatapointKey} data in {st.ElapsedMilliseconds}ms");
                        }
                    }
                }


            });

            //  jetstream.DeleteStreamAsync(streamName2);
            var stream = await jetstream.CreateStreamAsync(new StreamConfig(streamName2, new[] { $"{streamName}.>" })
            {
                Retention = StreamConfigRetention.Limits,
                MaxBytes = 400000000,
                MaxAge = TimeSpan.FromMilliseconds(5000),
                MaxMsgs = 100,
                Discard = StreamConfigDiscard.Old,
                Compression = StreamConfigCompression.S2,
                Storage = StreamConfigStorage.Memory
            });
            var consumer = await stream.CreateOrUpdateConsumerAsync(new ConsumerConfig("processor-1"));

            //            var sub = nats.SubscribeAsync<byte[]>(subject: "picture");
            var subscription = Task.Run(async () =>
            {
                await foreach (var msg in consumer.ConsumeAsync<byte[]>(opts: new NatsJSConsumeOpts { MaxMsgs = 1000 }))
                {
                    this.MessageQueue.Enqueue(msg);
                    Console.WriteLine($"Enqueued, Total: {x++}");
                    this.mMainThreadEvent.Set();
                    st.Start();
                    await msg.AckAsync();

                    // Console.WriteLine($"{x++}");
                }
            });
            //Task.Run(async () =>
            //{
            //    await foreach (var msg in nats.SubscribeAsync<byte[]>(subject: "picture2"))
            //    {
            //        this.MessageQueue.Enqueue(msg);

            //    }
            //});

            //Task.Run(async () =>
            //{
            //    await foreach (var msg in nats.SubscribeAsync<byte[]>(subject: "picture3"))
            //    {
            //        this.MessageQueue.Enqueue(msg);

            //    }
            //});

            //Task.Run(async () =>
            //{
            //    await foreach (var msg in nats.SubscribeAsync<byte[]>(subject: "picture4"))
            //    {
            //        this.MessageQueue.Enqueue(msg);

            //    }
            //});

            //Task.Run(async () =>
            //{
            //    await foreach (var msg in nats.SubscribeAsync<byte[]>(subject: "picture5"))
            //    {
            //        this.MessageQueue.Enqueue(msg);

            //    }
            //});

            //Task.Run(async () =>
            //{
            //    await foreach (var msg in nats.SubscribeAsync<byte[]>(subject: "picture6"))
            //    {
            //        this.MessageQueue.Enqueue(msg);

            //    }
            //});

            //Task.Run(async () =>
            //{
            //    await foreach (var msg in nats.SubscribeAsync<byte[]>(subject: "picture7"))
            //    {
            //        this.MessageQueue.Enqueue(msg);

            //    }
            //});

            //Task.Run(async () =>
            //{
            //    await foreach (var msg in nats.SubscribeAsync<byte[]>(subject: "picture8"))
            //    {
            //        this.MessageQueue.Enqueue(msg);

            //    }
            //});



            //var subscription2 = Task.Run(async () =>
            //{
            //    await foreach (var msg in nats.SubscribeAsync<byte[]>(subject: "picture2"))
            //    {
            //        TransportUnit message = ProtoHelper.DeserializeDecompressFromBytes(msg.Data);

            //        if (message != null)
            //            Console.WriteLine($"Received data {message.DatapointKey}");
            //    }
            //});

            //var subscription3 = Task.Run(async () =>
            //{
            //    await foreach (var msg in nats.SubscribeAsync<byte[]>(subject: "picture3"))
            //    {
            //        TransportUnit message = ProtoHelper.DeserializeDecompressFromBytes(msg.Data);

            //        if (message != null)
            //            Console.WriteLine($"Received data {message.DatapointKey}");
            //    }
            //});
        }
    }
}

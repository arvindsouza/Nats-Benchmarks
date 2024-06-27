using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commons;
using Commons.BusinessModels;
using NATS.Client;
using NATS.Client.Core;
using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;

namespace Nats_Test
{
    public class NatsPublisher
    {
        public NatsPublisher()
        {
            this.Publish();
        }

        public string DpKey { get; set; } = "1";
        INatsJSStream mStream;

        public void DeleteStream()
        {
            mStream.DeleteAsync();
        }
        public async void Publish()
        {
            // NatsConnection nats = new NatsConnection();
            string streamName = "test";
            string streamName2 = "TEST";
            int totalMessageCountPerThread = 10000;
            NatsConnection nats = new NatsConnection();
            int delay = 100;
            var jetstream = new NatsJSContext(nats);

            try
            {
                mStream = await jetstream.GetStreamAsync(streamName2);
            }
            catch (Exception ex)
            {
                mStream = await jetstream.CreateStreamAsync(new StreamConfig(streamName2, new[] { $"{streamName}.>" })
                {
                    Retention = StreamConfigRetention.Workqueue,
                    MaxBytes = 400000000,
                    Discard = StreamConfigDiscard.Old
                });
            }

            FileInfo img = new FileInfo("pictures\\images.jpg");
            byte[] file = File.ReadAllBytes("pictures\\images.jpg");

            Console.WriteLine(file.Length);
            //img.
            //File.ReadAllBytes(img)


            Task.Run(async () =>
            {

                for (int i = 0; i < totalMessageCountPerThread; i++)
                {
                    TransportUnit dataToSend = new TransportUnit();
                    dataToSend.DatapointKey = this.DpKey;
                    dataToSend.DatapointValue = file;
                    dataToSend.DatapointName = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                    var ack = await jetstream.PublishAsync<byte[]>($"{streamName}.picture", ProtoHelper.SerializeCompressedToBytes(dataToSend));
                    ack.EnsureSuccess(); Console.WriteLine($"Published at {dataToSend.DatapointKey} total: {i}");
                    Console.WriteLine($"Stream size {jetstream.GetStreamAsync(streamName2).Result.Info.State.Bytes}");

                }
            });

            Task.Run(async () =>
            {

                for (int i = 0; i < totalMessageCountPerThread; i++)
                {
                    TransportUnit dataToSend = new TransportUnit();
                    dataToSend.DatapointKey = 2.ToString();
                    dataToSend.DatapointValue = file;
                    dataToSend.DatapointName = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                    var ack = await jetstream.PublishAsync<byte[]>($"{streamName}.picture", ProtoHelper.SerializeCompressedToBytes(dataToSend));
                    ack.EnsureSuccess(); Console.WriteLine($"Published at {dataToSend.DatapointKey} total: {i}");

                }
            });

            Task.Run(async () =>
            {

                for (int i = 0; i < totalMessageCountPerThread; i++)
                {
                    TransportUnit dataToSend = new TransportUnit();
                    dataToSend.DatapointKey = 3.ToString();
                    dataToSend.DatapointValue = file;
                    dataToSend.DatapointName = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                    var ack = await jetstream.PublishAsync<byte[]>($"{streamName}.picture", ProtoHelper.SerializeCompressedToBytes(dataToSend));
                    ack.EnsureSuccess(); Console.WriteLine($"Published at {dataToSend.DatapointKey} total: {i}");

                }
            });

            Task.Run(async () =>
            {
                for (int i = 0; i < totalMessageCountPerThread; i++)
                {
                    // await Task.Delay(delay);
                    TransportUnit dataToSend = new TransportUnit();
                    dataToSend.DatapointKey = 4.ToString();
                    dataToSend.DatapointValue = file;
                    dataToSend.DatapointName = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                    var ack = await jetstream.PublishAsync<byte[]>($"{streamName}.picture", ProtoHelper.SerializeCompressedToBytes(dataToSend));
                    ack.EnsureSuccess(); Console.WriteLine($"Published at {dataToSend.DatapointKey} total: {i}");

                }
            });

            Task.Run(async () =>
            {
                for (int i = 0; i < totalMessageCountPerThread; i++)
                {
                    TransportUnit dataToSend = new TransportUnit();
                    dataToSend.DatapointKey = 5.ToString();
                    dataToSend.DatapointValue = file;
                    dataToSend.DatapointName = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                    var ack = await jetstream.PublishAsync<byte[]>($"{streamName}.picture", ProtoHelper.SerializeCompressedToBytes(dataToSend));
                    ack.EnsureSuccess(); Console.WriteLine($"Published at {dataToSend.DatapointKey} total: {i}");

                }
            });

            Task.Run(async () =>
            {
                for (int i = 0; i < totalMessageCountPerThread; i++)
                {
                    TransportUnit dataToSend = new TransportUnit();
                    dataToSend.DatapointKey = 6.ToString();
                    dataToSend.DatapointValue = file;
                    dataToSend.DatapointName = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                    var ack = await jetstream.PublishAsync<byte[]>($"{streamName}.picture", ProtoHelper.SerializeCompressedToBytes(dataToSend));
                    ack.EnsureSuccess(); Console.WriteLine($"Published at {dataToSend.DatapointKey} total: {i}");

                }
            });
            Task.Run(async () =>
            {
                for (int i = 0; i < totalMessageCountPerThread; i++)
                {
                    TransportUnit dataToSend = new TransportUnit();
                    dataToSend.DatapointKey = 7.ToString();
                    dataToSend.DatapointValue = file;
                    dataToSend.DatapointName = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                    var ack = await jetstream.PublishAsync<byte[]>($"{streamName}.picture", ProtoHelper.SerializeCompressedToBytes(dataToSend));
                    ack.EnsureSuccess();
                    Console.WriteLine($"Published at {dataToSend.DatapointKey} total: {i}");

                }
            });
            Task.Run(async () =>
            {
                for (int i = 0; i < totalMessageCountPerThread; i++)
                {
                    TransportUnit dataToSend = new TransportUnit();
                    dataToSend.DatapointKey = 8.ToString();
                    dataToSend.DatapointValue = file;
                    dataToSend.DatapointName = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                    var ack = await jetstream.PublishAsync<byte[]>($"{streamName}.picture", ProtoHelper.SerializeCompressedToBytes(dataToSend));
                    ack.EnsureSuccess();
                    Console.WriteLine($"Published at {dataToSend.DatapointKey} total: {i}");

                }
            });




            //Task.Run(() =>
            //{
            //    for (int i = 0; i < 10000; i++)
            //    {
            //        TransportUnit dataToSend = new TransportUnit();
            //        dataToSend.DatapointKey = 2.ToString();
            //        dataToSend.DatapointValue = file;
            //        dataToSend.DatapointName = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            //        nats.PublishAsync<byte[]>("picture2", ProtoHelper.SerializeCompressedToBytes(dataToSend));
            //        Console.WriteLine($"Published at {dataToSend.DatapointKey} total: {i}");

            //    }
            //});

            //Task.Run(() =>
            //{
            //    for (int i = 0; i < 10000; i++)
            //    {
            //        TransportUnit dataToSend = new TransportUnit();
            //        dataToSend.DatapointKey = 3.ToString();
            //        dataToSend.DatapointValue = file;
            //        dataToSend.DatapointName = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            //        nats.PublishAsync<byte[]>("picture3", ProtoHelper.SerializeCompressedToBytes(dataToSend));
            //        Console.WriteLine($"Published at {dataToSend.DatapointKey} total: {i}");

            //    }
            //});

            //Task.Run(() =>
            //{
            //    for (int i = 0; i < 10000; i++)
            //    {
            //        TransportUnit dataToSend = new TransportUnit();
            //        dataToSend.DatapointKey = 4.ToString();
            //        dataToSend.DatapointValue = file;
            //        dataToSend.DatapointName = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            //        nats.PublishAsync<byte[]>("picture4", ProtoHelper.SerializeCompressedToBytes(dataToSend));
            //        Console.WriteLine($"Published at {dataToSend.DatapointKey} total: {i}");

            //    }
            //});

            //Task.Run(() =>
            //{
            //    for (int i = 0; i < 10000; i++)
            //    {
            //        TransportUnit dataToSend = new TransportUnit();
            //        dataToSend.DatapointKey = 5.ToString();
            //        dataToSend.DatapointValue = file;
            //        dataToSend.DatapointName = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            //        nats.PublishAsync<byte[]>("picture5", ProtoHelper.SerializeCompressedToBytes(dataToSend));
            //        Console.WriteLine($"Published at {dataToSend.DatapointKey} total: {i}");

            //    }
            //});

            //Task.Run(() =>
            //{
            //    for (int i = 0; i < 10000; i++)
            //    {
            //        TransportUnit dataToSend = new TransportUnit();
            //        dataToSend.DatapointKey = 6.ToString();
            //        dataToSend.DatapointValue = file;
            //        dataToSend.DatapointName = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            //        nats.PublishAsync<byte[]>("picture6", ProtoHelper.SerializeCompressedToBytes(dataToSend));
            //        Console.WriteLine($"Published at {dataToSend.DatapointKey} total: {i}");

            //    }
            //});
            //Task.Run(() =>
            //{
            //    for (int i = 0; i < 10000; i++)
            //    {
            //        TransportUnit dataToSend = new TransportUnit();
            //        dataToSend.DatapointKey = 7.ToString();
            //        dataToSend.DatapointValue = file;
            //        dataToSend.DatapointName = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            //        nats.PublishAsync<byte[]>("picture7", ProtoHelper.SerializeCompressedToBytes(dataToSend));
            //        Console.WriteLine($"Published at {dataToSend.DatapointKey} total: {i}");

            //    }
            //});
            //Task.Run(() =>
            //{
            //    for (int i = 0; i < 10000; i++)
            //    {
            //        TransportUnit dataToSend = new TransportUnit();
            //        dataToSend.DatapointKey = 8.ToString();
            //        dataToSend.DatapointValue = file;
            //        dataToSend.DatapointName = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            //        nats.PublishAsync<byte[]>("picture8", ProtoHelper.SerializeCompressedToBytes(dataToSend));
            //        Console.WriteLine($"Published at {dataToSend.DatapointKey} total: {i}");

            //    }
            //});



        }

    }
}

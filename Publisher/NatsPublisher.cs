using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Commons;
using Commons.BusinessModels;
using NATS.Client;
using NATS.Client.Core;
using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;
using Serilog;
using Serilog.Core;

namespace Nats_Test
{
    public class NatsPublisher
    {

        public string DpKey { get; set; } = "1";

        // INatsJSStream mStream;
        // NatsJSContext mJetstream;
        string mFilePath = "pictures\\group.jpg";
        int mNoOfTasks = 8;

        Logger PublisherLog = new LoggerConfiguration()
            // .WriteTo.Console()
            .WriteTo.File("logs\\PublisherLog.log").CreateLogger();

        public NatsPublisher()
        {
            //this.GetStream();
        }

        public async Task PublishToSingleSubject()
        {
            var stopwatch = Stopwatch.StartNew();
            // NatsConnection nats = new NatsConnection();

            byte[] file = File.ReadAllBytes(mFilePath);

            Console.WriteLine(file.Length);
            //img.
            //File.ReadAllBytes(img)

            int counter = 0;

            var tasks = new List<Task>();
            while (counter < StreamDetails.NUMBER_OF_TASKS)
            {
                counter++;
                PublisherLog.Information($"task {counter}");
                tasks.Add(Task.Run(async () =>
                {
                    PublisherLog.Information($"start");
                    await using NatsConnection nats = new NatsConnection(new NatsOpts
                    {
                        SubPendingChannelFullMode = BoundedChannelFullMode.Wait,
                        SerializerRegistry = new MyProtoBufSerializerRegistry()
                    });
                    var mJetstream = new NatsJSContext(nats);
                    TransportUnit2 dataToSend = new TransportUnit2();
                    dataToSend.DatapointKey = counter.ToString();
                    for (int i = 0; i < StreamDetails.TOTAL_MESSAGES_PER_TASK; i++)
                    {
                        dataToSend.DatapointValue = file;
                        dataToSend.DatapointName = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);

                        var ack = await mJetstream.PublishAsync<TransportUnit2>($"{StreamDetails.SUBJECT_NAME}.picture", dataToSend);
                        ack.EnsureSuccess();
                        //  Console.WriteLine($"Published at {dataToSend.DatapointKey} total: {i}");
                       // PublisherLog.Information($"Stream size {mJetstream.GetStreamAsync(StreamDetails.STREAM_NAME).Result.Info.State.Bytes}");

                    }
                    Console.WriteLine($"end");
                }));
            }

            await Task.WhenAll(tasks);
            PublisherLog.Information($"all done {stopwatch.Elapsed}");
        }

        public async void PublishToSingleSubjectWithOneTask()
        {
            var stopwatch = Stopwatch.StartNew();

            byte[] file = File.ReadAllBytes(mFilePath);

            Console.WriteLine(file.Length);

            int counter = 0;

            List<Task> tasks = new List<Task>();
            tasks.Add(Task.Run(async () =>
            {
                await using NatsConnection nats = new NatsConnection(new NatsOpts
                {
                    SubPendingChannelFullMode = BoundedChannelFullMode.Wait,
                });
                var mJetstream = new NatsJSContext(nats);
                int taskNo = counter++;
                for (int i = 0; i < StreamDetails.TOTAL_MESSAGES_PER_TASK; i++)
                {
                    TransportUnit dataToSend = new TransportUnit();
                    dataToSend.DatapointKey = i.ToString();
                    dataToSend.DatapointValue = file;
                    dataToSend.DatapointName = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                    var ack = await mJetstream.PublishAsync<byte[]>($"{StreamDetails.SUBJECT_NAME}.picture", ProtoHelper.SerializeCompressedToBytes(dataToSend));
                    ack.EnsureSuccess();
                    PublisherLog.Information($"Published at {dataToSend.DatapointKey} total: {i}");
                //    Console.WriteLine($"Stream size {mJetstream.GetStreamAsync(StreamDetails.STREAM_NAME).Result.Info.State.Bytes}");

                }
            }));
            await Task.WhenAll(tasks);
            PublisherLog.Information($"all done {stopwatch.Elapsed}");

        }

        public async void PublishToMultipleSubject()
        {
            byte[] file = File.ReadAllBytes(mFilePath);
            var stopwatch = Stopwatch.StartNew();

            int counter = 0;
            List<Task> tasks = new List<Task>();

            while (counter < StreamDetails.NUMBER_OF_TASKS)
            {
                TransportUnit dataToSend = new TransportUnit();
                int taskNo = counter++;
                tasks.Add(Task.Run(async () =>
                {
                    await using NatsConnection nats = new NatsConnection(new NatsOpts
                    {
                        SubPendingChannelFullMode = BoundedChannelFullMode.Wait,
                    });
                    var mJetstream = new NatsJSContext(nats);
                    try
                    {
                        for (int i = 0; i < StreamDetails.TOTAL_MESSAGES_PER_TASK; i++)
                        {
                            dataToSend.DatapointKey = i.ToString();
                            dataToSend.DatapointValue = file;
                            dataToSend.DatapointName = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                            var ack = await mJetstream.PublishAsync<byte[]>($"{StreamDetails.SUBJECT_NAME}.picture{taskNo}", ProtoHelper.SerializeCompressedToBytes(dataToSend));
                            ack.EnsureSuccess();

                            PublisherLog.Information($" {ack.IsSuccess()} Published at {dataToSend.DatapointKey} total: {i}, SubjectName {StreamDetails.SUBJECT_NAME}.picture{taskNo}");
                            //  Console.WriteLine($"Stream size {mJetstream.GetStreamAsync(StreamDetails.STREAM_NAME).Result.Info.State.Bytes}");

                        }
                    }
                    catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                }));
            }

            await Task.WhenAll(tasks);
            PublisherLog.Information($"all done {stopwatch.Elapsed}");

        }


    }
}

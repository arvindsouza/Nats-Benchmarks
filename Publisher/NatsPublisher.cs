using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
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

        public string DpKey { get; set; } = "1";

        INatsJSStream mStream;
        NatsJSContext mJetstream;
        string mFilePath = "pictures\\group.jpg";
        int mNoOfTasks = 8;


        public NatsPublisher()
        {
            this.GetStream();
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

        public void PublishToSingleSubject()
        {
            // NatsConnection nats = new NatsConnection();
            int delay = 100;


            byte[] file = File.ReadAllBytes(mFilePath);

            Console.WriteLine(file.Length);
            //img.
            //File.ReadAllBytes(img)

            int counter = 0;

            while (counter < StreamDetails.NUMBER_OF_TASKS)
            {
                counter++;
                TransportUnit dataToSend = new TransportUnit();
                dataToSend.DatapointKey = counter.ToString();
                Task.Run(async () =>
                {
                    for (int i = 0; i < StreamDetails.TOTAL_MESSAGES_PER_TASK; i++)
                    {
                        dataToSend.DatapointValue = file;
                        dataToSend.DatapointName = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                        var ack = await mJetstream.PublishAsync<byte[]>($"{StreamDetails.SUBJECT_NAME}.picture", ProtoHelper.SerializeCompressedToBytes(dataToSend));
                        ack.EnsureSuccess(); 
                      //  Console.WriteLine($"Published at {dataToSend.DatapointKey} total: {i}");
                    //    Console.WriteLine($"Stream size {mJetstream.GetStreamAsync(StreamDetails.STREAM_NAME).Result.Info.State.Bytes}");

                    }
                });
            }

        }

        public void PublishToSingleSubjectWithOneTask()
        {
            int delay = 100;

            byte[] file = File.ReadAllBytes(mFilePath);

            Console.WriteLine(file.Length);
            //img.
            //File.ReadAllBytes(img)

            int counter = 0;


            Task.Run(async () =>
            {
                int taskNo = counter++;
                for (int i = 0; i < StreamDetails.TOTAL_MESSAGES_PER_TASK; i++)
                {
                    TransportUnit dataToSend = new TransportUnit();
                    dataToSend.DatapointKey = i.ToString();
                    dataToSend.DatapointValue = file;
                    dataToSend.DatapointName = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                    var ack = await mJetstream.PublishAsync<byte[]>($"{StreamDetails.SUBJECT_NAME}.picture", ProtoHelper.SerializeCompressedToBytes(dataToSend));
                    ack.EnsureSuccess(); 
                    Console.WriteLine($"Published at {dataToSend.DatapointKey} total: {i}");
                //    Console.WriteLine($"Stream size {mJetstream.GetStreamAsync(StreamDetails.STREAM_NAME).Result.Info.State.Bytes}");

                }
            });

        }

        public void PublishToMultipleSubject()
        {
            byte[] file = File.ReadAllBytes(mFilePath);

            int counter = 0;
            while (counter < StreamDetails.NUMBER_OF_TASKS)
            {
                TransportUnit dataToSend = new TransportUnit();
                int taskNo = counter++;
                Task.Run(async () =>
                {
                    for (int i = 0; i < StreamDetails.TOTAL_MESSAGES_PER_TASK; i++)
                    {
                        dataToSend.DatapointKey = i.ToString();
                        dataToSend.DatapointValue = file;
                        dataToSend.DatapointName = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                        var ack = await mJetstream.PublishAsync<byte[]>($"{StreamDetails.SUBJECT_NAME}.picture{taskNo}", ProtoHelper.SerializeCompressedToBytes(dataToSend));
                        ack.EnsureSuccess(); 
                        Console.WriteLine($"Published at {dataToSend.DatapointKey} total: {i}");
                      //  Console.WriteLine($"Stream size {mJetstream.GetStreamAsync(StreamDetails.STREAM_NAME).Result.Info.State.Bytes}");

                    }
                });
            }

        }


    }
}

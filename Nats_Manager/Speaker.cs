using Commons;
using NATS.Client.Core;
using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Nats_Manager
{
    public class Speaker
    {
        INatsJSStream mStream;
        NatsJSContext mJetstream;

        private static Speaker mInstance;
        public static Speaker Instance
        {
            get
            {
                if(mInstance == null)
                    mInstance = new Speaker();
                return mInstance;

            }
            set
            {

                mInstance = value;

            }
        }

        public Speaker()
        {
            NatsConnection nats = new NatsConnection();
            this.mJetstream = new NatsJSContext(nats);
            Instance = this;
        }

        public async void CreateInterestStream()
        {
            mStream = await mJetstream.CreateStreamAsync(new StreamConfig(StreamDetails.STREAM_NAME, new[] { $"{StreamDetails.SUBJECT_NAME}.>" })
            {
                Retention = StreamConfigRetention.Interest,
                MaxBytes = StreamDetails.STREAM_SIZE_LIMIT,
                Discard = StreamConfigDiscard.Old
            });
        }

        public async void CreateWorkQueueStream()
        {
            mStream = await mJetstream.CreateStreamAsync(new StreamConfig(StreamDetails.STREAM_NAME, new[] { $"{StreamDetails.SUBJECT_NAME}.>" })
            {
                Retention = StreamConfigRetention.Workqueue,
                MaxBytes = StreamDetails.STREAM_SIZE_LIMIT,
                Discard = StreamConfigDiscard.Old
            });
        }

        public void Dispose()
        {
            mStream.DeleteAsync();
        }
    }
}

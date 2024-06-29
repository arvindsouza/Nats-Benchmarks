using Google.Protobuf;
using Google.Protobuf.Reflection;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Commons.BusinessModels
{
    public class TransportUnit2 : IMessage<TransportUnit2>, IBufferMessage
    {
        public static readonly MessageParser<TransportUnit2> Parser = new(() => new TransportUnit2());

        public string DatapointKey { get; set; }

        public string DatapointName { get; set; }

        public byte[] DatapointValue { get; set; }

        public void MergeFrom(TransportUnit2 message) {
            this.DatapointKey = message.DatapointKey;
            this.DatapointValue = message.DatapointValue;
            this.DatapointName = message.DatapointName;
        }


        public void MergeFrom(CodedInputStream input)
        {
            uint tag;
            while ((tag = input.ReadTag()) != 0)
            {
                if (tag == 10)
                    DatapointName = input.ReadString();

                if (tag == 11)
                    DatapointKey = input.ReadString();

                if (tag == 12)
                    DatapointValue = input.ReadBytes().ToArray();
            }
        }


        public void WriteTo(CodedOutputStream output)
        {
            output.WriteRawTag(10);
            output.WriteString(DatapointName);
            output.WriteRawTag(11);
            output.WriteString(DatapointKey);
            output.WriteRawTag(12);
            output.WriteBytes(ByteString.CopyFrom(DatapointValue));

        }


        public int CalculateSize() => CodedOutputStream.ComputeStringSize(DatapointName) + CodedOutputStream.ComputeStringSize(DatapointKey) + CodedOutputStream.ComputeBytesSize(ByteString.CopyFrom(DatapointValue)) + 1;


        public MessageDescriptor Descriptor => null!;


        public bool Equals(TransportUnit2 other) => string.Equals(other?.DatapointName, DatapointName);


        public TransportUnit2 Clone() => new() { DatapointName = DatapointName, DatapointKey = DatapointKey, DatapointValue = DatapointValue };


        public void InternalMergeFrom(ref ParseContext input)
        {
            uint tag;
            while ((tag = input.ReadTag()) != 0)
            {
                if (tag == 10)
                    DatapointName = input.ReadString();

                if(tag == 11)
                {
                    DatapointKey = input.ReadString();
                }

                if(tag==12) {
                    DatapointValue = input.ReadBytes().ToArray();
                }
            }
        }


        public void InternalWriteTo(ref WriteContext output)
        {
            output.WriteRawTag(10);
            output.WriteString(DatapointName);
            output.WriteRawTag(11);
            output.WriteString(DatapointKey);
            output.WriteRawTag(12);
            output.WriteBytes(ByteString.CopyFrom(DatapointValue));
        }
    }
}

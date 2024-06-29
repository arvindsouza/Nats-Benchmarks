using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons
{
    public class GreetRequest : IMessage<GreetRequest>, IBufferMessage
    {
        public static readonly MessageParser<GreetRequest> Parser = new(() => new GreetRequest());


        public string Name { get; set; }


        public void MergeFrom(GreetRequest message) => Name = message.Name;


        public void MergeFrom(CodedInputStream input)
        {
            uint tag;
            while ((tag = input.ReadTag()) != 0)
            {
                if (tag == 10)
                    Name = input.ReadString();
            }
        }


        public void WriteTo(CodedOutputStream output)
        {
            output.WriteRawTag(10);
            output.WriteString(Name);
        }


        public int CalculateSize() => CodedOutputStream.ComputeStringSize(Name) + 1;


        public MessageDescriptor Descriptor => null!;


        public bool Equals(GreetRequest other) => string.Equals(other?.Name, Name);


        public GreetRequest Clone() => new() { Name = Name };


        public void InternalMergeFrom(ref ParseContext input)
        {
            uint tag;
            while ((tag = input.ReadTag()) != 0)
            {
                if (tag == 10)
                    Name = input.ReadString();
            }
        }


        public void InternalWriteTo(ref WriteContext output)
        {
            output.WriteRawTag(10);
            output.WriteString(Name);
        }
    }
}

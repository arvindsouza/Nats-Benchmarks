using Commons.BusinessModels;
using Google.Protobuf;
using NATS.Client.Core;
using ProtoBuf;
using System;
using ProtoBuf;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Buffers;

namespace Commons
{
    public class NatsCustomSerializer<T>: INatsSerializer<T>
    {
        public static readonly INatsSerializer<T> Default = new NatsCustomSerializer<T>();

        public void Serialize(IBufferWriter<byte> bufferWriter, T value)
        {
            if (value is IMessage message)
            {
                message.WriteTo(bufferWriter);
            }
            else
            {
                throw new NatsException($"Can't serialize {typeof(T)}");
            }
        }

        public T? Deserialize(in ReadOnlySequence<byte> buffer)
        {
            if (typeof(T) == typeof(TransportUnit2))
            {
                return (T)(object)TransportUnit2.Parser.ParseFrom(buffer);
            }

            throw new NatsException($"Can't deserialize {typeof(T)}");
        }
    }

    public class MyProtoBufSerializerRegistry : INatsSerializerRegistry
    {
        public INatsSerialize<T> GetSerializer<T>() => NatsCustomSerializer<T>.Default;

        public INatsDeserialize<T> GetDeserializer<T>() => NatsCustomSerializer<T>.Default;
    }
}


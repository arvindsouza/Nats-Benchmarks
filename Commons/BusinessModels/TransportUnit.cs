using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.BusinessModels
{
    [ProtoContract]
    public class TransportUnit
    {
        [ProtoMember(1)]
        public string DatapointKey { get; set; }

        [ProtoMember(2)]
        public byte[] DatapointByteValue { get; set; }

        [ProtoMember(3)]
        public string DatapointStringValue { get; set; }


        [ProtoMember(4)]
        public string DatapointName { get; set; }

        [ProtoMember(5)]
        public byte[] DatapointValue { get; set; }
    }
}

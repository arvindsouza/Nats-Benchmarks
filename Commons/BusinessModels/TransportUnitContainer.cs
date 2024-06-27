using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.BusinessModels
{
    [ProtoContract]
    public class TransportUnitContainer
    {
        [ProtoMember(1)]
        public List<TransportUnit> TransportUnits { get; set; }
    }
}

using Commons.BusinessModels;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons
{
    public class ProtoHelper
    {
        public static byte[] SerializeCompressedToBytes(TransportUnit transportUnit)
        {
            byte[] byteA = null;
            MemoryStream memoryStream = new MemoryStream();
            try
            {
                Serializer.SerializeWithLengthPrefix(memoryStream, transportUnit, PrefixStyle.Base128, 1);

                byteA = memoryStream.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                try
                {
                    memoryStream.Close();
                }
                catch (Exception)
                {
                }
            }

            return byteA;
        }

        public static byte[] SerializeCompressedToBytes(TransportUnitContainer transportUnit)
        {
            byte[] byteA = null;
            MemoryStream memoryStream = new MemoryStream();
            try
            {
                Serializer.SerializeWithLengthPrefix(memoryStream, transportUnit, PrefixStyle.Base128, 1);

                byteA = memoryStream.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                try
                {
                    memoryStream.Close();
                }
                catch (Exception)
                {
                }
            }

            return byteA;
        }

        public static TransportUnit DeserializeDecompressFromBytes(byte[] byteArray)
        {
            TransportUnit objReturn = null;
            MemoryStream stream = null;

            try
            {
                stream = new MemoryStream(byteArray);
                objReturn = Serializer.DeserializeWithLengthPrefix<TransportUnit>(stream, PrefixStyle.Base128, 1);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                try
                {
                    if (stream != null)
                    {
                        stream.Close();
                    }
                }
                catch (Exception)
                {
                }
            }

            return objReturn;
        }

        public static TransportUnitContainer DeserializeDecompressContainerFromBytes(byte[] byteArray)
        {
            TransportUnitContainer objReturn = null;
            MemoryStream stream = null;

            try
            {
                stream = new MemoryStream(byteArray);
                objReturn = Serializer.DeserializeWithLengthPrefix<TransportUnitContainer>(stream, PrefixStyle.Base128, 1);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                try
                {
                    if (stream != null)
                    {
                        stream.Close();
                    }
                }
                catch (Exception)
                {
                }
            }

            return objReturn;
        }

        //public static TransportUnit ConvertFromDataParameterDBMToParameterTransportDataUnit(DataPoint dataParameterDBM)
        //{
        //    TransportUnit parameterTransportUnit = new TransportUnit();

        //    parameterTransportUnit.DatapointKey = dataParameterDBM.Key;

        //    if(dataParameterDBM.Value.GetType() == typeof(byte[]))
        //    {
        //        parameterTransportUnit.DatapointByteValue = dataParameterDBM.Value as byte[];
        //    }
        //    else
        //    {
        //        parameterTransportUnit.DatapointStringValue = dataParameterDBM.Value.ToString();
        //    }

        //    return parameterTransportUnit;
        //}

        //public static List<TransportUnit> ConvertFromDataParameterDBMToParameterTransportDataUnit(List<DataPoint> dataParameters)
        //{
        //    List<TransportUnit> parameterTransportUnit = new List<TransportUnit>();

        //    foreach(DataPoint dp in dataParameters)
        //    {
        //        TransportUnit unit = new TransportUnit();
        //        unit.DatapointKey = dp.Key;
        //        if (dp.Value.GetType() == typeof(byte[]))
        //        {
        //            unit.DatapointByteValue = dp.Value as byte[];
        //        }
        //        else
        //        {
        //            unit.DatapointStringValue = dp.Value.ToString();
        //        }
        //        parameterTransportUnit.Add(unit);
        //    }

        //    return parameterTransportUnit;
        //}

    }
}

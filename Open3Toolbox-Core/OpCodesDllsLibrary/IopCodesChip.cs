using System;
using System.Collections.Generic;
using Biss.Interfaces;
using ExConfigExchange.JsonUtils;

namespace OpCodesDllsLibrary
{
    public interface IopCodesChip:IBissSerialize, IDeserializable
    {
        public (byte[], byte[]) GetOpCodes(byte port); 

        public string ChipType { get; set; }
        public InterfaceType InterfaceType { get; set; }
        public double ParsingReadedValueToValue(double data, int port);

        public double ParsingInputedValueToSendValue(double data, int port);

        public void ConvertInputConfigOptions(Dictionary<string, object> inputs);

        public Dictionary<string, Type> GetConfigOptions();
    }
    
}

using System;
using Newtonsoft.Json;

namespace Newtonsoft.Tests
{
    public class DtoTestObject
    {
        [JsonProperty(PropertyName = "TDT", Required = Required.Always)]
        public DateTime Tdt { get; set; }
    }
}
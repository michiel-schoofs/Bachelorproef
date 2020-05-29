using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace GenerateContractsJsonFile.Model {
    public class Contract {
        public string contractName { get; set; }
        public string bytecode { get; set; }
        public IDictionary<string, JToken> ast { get; set; }
    }
}
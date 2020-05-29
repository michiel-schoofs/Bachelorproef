using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace Console_Application.Contracts.Contract {
   [Function("getName", "string")]
   public class GetNameOfRepo : FunctionMessage { }
   [Function("getId", "uint256")]
   public class GetIdOfRepo : FunctionMessage { }
   [Function("getCid","string")]
   public class GetCidOfRepo : FunctionMessage { }

   [Function("setCid", "bool")]
    public class SetCidFunction : FunctionMessage{
        [Parameter("string", "_cid", 1)]
        public string Cid { get; set; }
    }

    [Function("getVersion", "string")]
    public class GetVersionFunction : FunctionMessage {
        [Parameter("uint256", "indx", 1)]
        public int Indx { get; set; }
    }

    [Function("getVersionCount", "uint256")]
    public class GetAllVersionCount : FunctionMessage { }
}

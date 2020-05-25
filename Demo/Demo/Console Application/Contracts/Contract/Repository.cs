using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace Console_Application.Contracts.Contract {
   [Function("getName", "string")]
   public class GetNameOfRepo : FunctionMessage { }
   [Function("getId", "uint256")]
   public class GetIdOfRepo : FunctionMessage { }
}

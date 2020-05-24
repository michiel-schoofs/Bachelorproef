using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace Console_Application.Contracts.Contract {
   [Function("name", "string")]
   public class GetNameOfRepo : FunctionMessage { }
   [Function("id", "uint256")]
   public class GetIdOfRepo : FunctionMessage { }
}

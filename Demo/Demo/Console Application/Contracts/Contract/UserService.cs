using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace Console_Application.Contracts.Contract {
    [Function("userHasAccount", "bool")]
    public class UserHasAccountFunction : FunctionMessage { }
}

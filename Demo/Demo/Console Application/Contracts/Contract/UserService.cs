using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace Console_Application.Contracts.Contract {
    [Function("userHasAccount", "bool")]
    public class UserHasAccountFunction : FunctionMessage { }

    [Function("addUser")]
    public class AddUserFunction : FunctionMessage { 
        [Parameter("string", "_username", 1, false)]
        public string Username { get; set; }
    }

    [Event("NewUserAdded")]
    public class NewUserAddedEvent : IEventDTO { 
        [Parameter("string", "_username")]
        public string Username { get; set; }
    }
}

using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace Console_Application.Contracts.Contract {
    [Function("userHasAccount", "bool")]
    public class UserHasAccountFunction : FunctionMessage { }

    [Function("usernameExists", "bool")]
    public class UsernameExists : FunctionMessage { 
        [Parameter("string","_username",1)]
        public string Username { get; set; }
    }

    [Function("addUser")]
    public class AddUserFunction : FunctionMessage { 
        [Parameter("string", "_username", 1, false)]
        public string Username { get; set; }
    }

    [Function("GetUsernameFromUser", "string")]
    public class GetUsernameFunction : FunctionMessage { }

    [Event("NewUserAdded")]
    public class NewUserAddedEvent : IEventDTO { 
        [Parameter("string", "_username")]
        public string Username { get; set; }
    }

    [Function("RequireTest")]
    public class RequireTestFunction : FunctionMessage { }

    [Function("returnRepositoriesByUser","address[]")]
    public class ReturnRepositoriesByUserFunction  : FunctionMessage {
        [Parameter("string", "_username",1,false)]
        public string Username { get; set; }
    }
}

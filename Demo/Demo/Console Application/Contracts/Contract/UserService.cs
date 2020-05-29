using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using System.Collections.Generic;

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

    public partial class ReturnUserFunction : ReturnUserFunctionBase { }

    [Function("ReturnUser", typeof(ReturnUserOutputDTO))]
    public class ReturnUserFunctionBase : FunctionMessage {
        [Parameter("string", "_username", 1)]
        public virtual string Username { get; set; }
    }

    public partial class ReturnUserOutputDTO : ReturnUserOutputDTOBase { }

    [FunctionOutput]
    public class ReturnUserOutputDTOBase : IFunctionOutputDTO {
        [Parameter("tuple", "", 1)]
        public virtual User ReturnValue1 { get; set; }
    }

    public partial class User : UserBase { }

    public class UserBase {
        [Parameter("address", "_address", 1)]
        public virtual string Address { get; set; }
        [Parameter("address[]", "_repositories", 2)]
        public virtual List<string> Repositories { get; set; }
    }
}

using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace Console_Application.Contracts.Contract {
    [Function("CreateRepository")]
    public class CreateRepositoryFunction : FunctionMessage {
        [Parameter("string", "name", 1)]
        public string Name { get; set; }
    }

    [Event("RepositoryAdded")]
    public class RepositoryAddedEvent : IEventDTO { 
        [Parameter("uint256", "_id")]
        public int Id { get; set; }
    }
}

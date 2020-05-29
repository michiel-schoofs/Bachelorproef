using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace Console_Application.Contracts.Contract {
    [Function("CreateRepository")]
    public class CreateRepositoryFunction : FunctionMessage {
        [Parameter("string", "name", 1)]
        public string Name { get; set; }
        [Parameter("string", "cid", 2)]
        public string Cid { get; set; }
    }

    [Function("CheckIfRepoExists", "bool")]
    public class CheckIfRepoExistsFunction : FunctionMessage {
        [Parameter("string", "name", 1)]
        public string Name { get; set; }
    }

    [Event("RepositoryAdded")]
    public class RepositoryAddedEvent : IEventDTO {
        [Parameter("uint256", "_id")]
        public int Id { get; set; }
    }

    [Function("getRepository", "address")]
    public class GetRepositoryFunction : FunctionMessage {
        [Parameter("string", "name", 1)]
        public string Name { get; set; }
    }
}
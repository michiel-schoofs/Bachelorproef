using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace Console_Application.Contracts.Deployment {
    public class RepositoryServiceContract : ContractDeploymentMessage {
        public static string Bytecode { get; set; }

        [Parameter("address", "_UserService")]
        public string UserServiceAddress { get; set; }

        public RepositoryServiceContract() : base(Bytecode) {}
    }
}

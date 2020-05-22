using Nethereum.Contracts;

namespace Console_Application.Contracts.Deployment {
    public class NoParameterContract : ContractDeploymentMessage {
        public static string Bytecode { get; set; }
        public NoParameterContract():base(Bytecode) {}
    }
}

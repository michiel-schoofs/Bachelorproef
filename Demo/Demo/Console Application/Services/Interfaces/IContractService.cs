using Nethereum.Web3;
using System.Threading.Tasks;

namespace Console_Application.Services.Interfaces {
    public interface IContractService {
        public string GetByteCode(string name);
        public string[] GetContractsNames();
        public bool ContractExist(string name);
        public bool ContractDeployed(string name);
        public string GetAddressDeployedContract(string name);
        public string[] GetDeployedContractsNames();
        public Task DeployContracts(Web3 web3,string path);
    }
}

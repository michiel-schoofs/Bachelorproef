using Console_Application.Contracts.Deployment;
using Console_Application.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Console_Application.Services.ContractService {
    public class ContractService : IContractService {
        private readonly ILogger<Program> _logger;
        private IDictionary<string, string> _contracts;
        private IDictionary<string, string> _deployed = new Dictionary<string,string>();
        public string FilePath { get; set; }

        public ContractService(ILoggerFactory loggerFactory, string filePath) {
            _logger = loggerFactory.CreateLogger<Program>();
            FilePath = filePath;
            Initialize();
        }

        private void Initialize() {
            try {

                FileStream file = File.OpenRead(FilePath);

                using (StreamReader reader = new StreamReader(file)) {
                    string json = reader.ReadToEnd();
                    _contracts = JsonConvert.DeserializeObject<IDictionary<string, string>>(json);
                }

                file.Close();

            } catch (Exception e) {
                _logger.LogError(e.Message);
                Console.WriteLine("Something went wrong with the contracts parsing file");
                Console.Beep();
                System.Environment.Exit(1);
            }
        }

        public string GetByteCode(string name) {
            _contracts.TryGetValue(name, out string val);
            return val;
        }

        public string[] GetContractsNames() {
            return _contracts.Keys.ToArray();
        }

        public bool ContractExist(string name) {
            return _contracts.ContainsKey(name);
        }

        public bool ContractDeployed(string name) {
            return _deployed.ContainsKey(name);
        }

        public string GetAddressDeployedContract(string name) {
            _deployed.TryGetValue(name, out string val);
            return val;
        }

        public string[] GetDeployedContractsNames() {
            return _deployed.Keys.ToArray();
        }

        public async Task DeployContracts(Web3 web3,string path) {
            try {

                if (File.Exists(path)) {
                    char[] validResponses = new char[]{'y','n'};
                    char response='n';
                    do {
                        Console.Write("Do you want to redeploy contracts (y,n)? ");
                        response = Console.ReadKey().KeyChar;
                    } while (!validResponses.Contains(response));

                    if (response.Equals('n')) {
                        FileStream stream = File.OpenRead(path);
                        using (StreamReader reader = new StreamReader(stream)) {
                            string json = reader.ReadToEnd();
                            _deployed = JsonConvert.DeserializeObject<IDictionary<string, string>>(json);
                        }
                        stream.Close();
                        return;
                    }
                } 

                foreach (string byteCode in _contracts.Values) {
                    string contractName = _contracts.Keys.ToList()[_contracts.Values.ToList().IndexOf(byteCode)];
                    _logger.LogInformation("Attempting deployment of {0}",contractName);

                    NoParameterContract.Bytecode = byteCode;
                    NoParameterContract contract = new NoParameterContract();
                    var deploymentHandler = web3.Eth.GetContractDeploymentHandler<NoParameterContract>();
                    TransactionReceipt transactionReceipt = await deploymentHandler.SendRequestAndWaitForReceiptAsync(contract);
                    CheckIfDeploymentWasSuccesfull(transactionReceipt);

                   
                    string address = transactionReceipt.ContractAddress;
                    _deployed.Add(contractName, address);
                    _logger.LogInformation("Deployment of {0} succesfull",contractName);
                }

                SaveToFile(path);
            } catch (Exception e) {
                _logger.LogError("Something went wrong with the deployment: "+e.Message);
                Console.WriteLine("Something went wrong with deployment of contracts");
                Console.Beep();
                System.Environment.Exit(1);
            }
        }

        private void SaveToFile(string path) {
            if (File.Exists(path)) {
                _logger.LogError("Deployed Contracts already exits, removing file");
                File.Delete(path);
            }

            FileStream stream = File.Create(path);

            using (StreamWriter writer = new StreamWriter(stream)) {
                string json = JsonConvert.SerializeObject(_deployed);
                writer.Write(json);
            }

            stream.Close();
        }

        private void CheckIfDeploymentWasSuccesfull(TransactionReceipt transactionReceipt) {
            if (transactionReceipt.IsContractAddressEmptyOrEqual("0x0") || transactionReceipt.Failed())
                throw new TransactionException("Something went wrong with the deployment");
        }
    }
}

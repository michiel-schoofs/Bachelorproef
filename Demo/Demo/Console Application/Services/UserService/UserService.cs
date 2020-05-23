using Console_Application.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Nethereum.RPC.Accounts;
using System;
using Console_Application.Extension;
using System.Linq;
using Nethereum.KeyStore.Crypto;
using System.Threading.Tasks;
using Nethereum.Web3;
using Nethereum.RPC.Eth.Exceptions;
using Console_Application.Contracts.Contract;
using Nethereum.Hex.HexTypes;

namespace Console_Application.Services.UserService {
    public class UserService : IUserService {
        private readonly IKeyStoreService _keyStoreService;
        private readonly ILogger<Program> _logger;
        private static Web3 _web3;
        private readonly IContractService _contractService;

        public UserService(IKeyStoreService keyStore, ILogger<Program> logger, IContractService contractService) {
            _keyStoreService = keyStore;
            _logger = logger;
            _contractService = contractService;
            _logger.LogInformation("User Service initialized");
        }

        public Web3 GetUser() {
            Console.Write("Provide the password of your keystore: ");
            string password = Console.ReadLine();

            try {
                if (_web3 == null)
                   _web3 = new Web3(_keyStoreService.GetAccount(password), url: "http://127.0.0.1:7545");
                
                return _web3;
            } catch (DecryptionException) {

                Console.WriteLine("Invalid password");
                Console.Beep();

                _logger.LogError("Invalid password, application shutting down");
                System.Environment.Exit(1);

                return null;
            }
        }

        public bool HasAccount() {
            bool hasKeys = _keyStoreService.HasKeyStore();

            if (!hasKeys)
                _logger.LogWarning("No keystore present");

            return hasKeys;
        }

        public Web3 RegisterAccount() {
            Console.Write("Please provide a password for future use: ");
            string password = Console.ReadLine();
            byte[] pk;
            do {
                Console.Write("Please enter your private key: ");

                try {
                    pk = Console.ReadLine().SplitInParts(2).Select(x => Convert.ToByte(x, 16)).ToArray();
                } catch (Exception e) {
                    pk = new byte[0];
                }

                if (pk.Length != 32) {
                    _logger.LogError("The size of the private key is invalid");
                    Console.Beep();
                    Console.WriteLine("Please enter a valid key");
                }

            } while (pk.Length != 32);

            Console.Write("Please enter your wallet address: ");
            string address = Console.ReadLine();

            _keyStoreService.GenerateKeyStore(password, pk, address);
            _web3 = new Web3(_keyStoreService.GetAccount(password), url: "http://127.0.0.1:7545");
            return _web3;
        }

        public async Task<bool> HasUsername() {
            if (!_contractService.ContractDeployed("UserService"))
                throw new Exception("The contract is not deployed");

            UserHasAccountFunction function = new UserHasAccountFunction();
            var functionHandler = _web3.Eth.GetContractQueryHandler<UserHasAccountFunction>();
            return await functionHandler.QueryAsync<bool>(
                _contractService.GetAddressDeployedContract("UserService"), function
            );
        }

        public async Task GetUsername() {
            try {
                bool b = await HasUsername();
                Console.WriteLine(b);
            } catch (Exception e) {
                _logger.LogError("Something went wrong with the GetUsername function: {0}", e.Message);
                Console.WriteLine("Something went wrong with the GetUsername function");
                Console.Beep();
                System.Environment.Exit(1);
            }
        }

        public async Task RegisterUsername() {
            try {
                string username;

                if (!_contractService.ContractDeployed("UserService"))
                    throw new Exception("The contract is not deployed");

                string contract_ad = _contractService.GetAddressDeployedContract("UserService");

                do {
                    Console.Write("No username registered, Please provide a username: ");
                    username = Console.ReadLine();
                } while (string.IsNullOrEmpty(username));

                AddUserFunction addFunction = new AddUserFunction() {
                    Username = username
                };

                var _functionHandler = _web3.Eth.GetContractTransactionHandler<AddUserFunction>();
                HexBigInteger gasprice = await _functionHandler.EstimateGasAsync(contract_ad, addFunction);

                _logger.LogInformation("Registering username for {0} gas", gasprice.Value.ToString());
                _functionHandler.SendRequestAndWaitForReceiptAsync(contract_ad, addFunction);
            } catch (Exception e) {
                _logger.LogError("Something went wrong with the RegisterUsername function: {0}", e.Message);
                Console.WriteLine("Something went wrong with Registering a username");
                Console.Beep();
                System.Environment.Exit(1);
            }
        }
    }
}


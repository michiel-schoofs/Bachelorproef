using Console_Application.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Nethereum.RPC.Accounts;
using System;
using Console_Application.Extension;
using System.Linq;
using Nethereum.Model;
using Nethereum.Web3.Accounts;
using Account = Nethereum.Web3.Accounts.Account;
using Nethereum.KeyStore.Crypto;

namespace Console_Application.Services.UserService {
    public class UserService : IUserService {
        private readonly IKeyStoreService _keyStoreService;
        private readonly ILogger<Program> _logger;

        public UserService(IKeyStoreService keyStore, ILogger<Program> logger) {
            _keyStoreService = keyStore;
            _logger = logger;
            _logger.LogInformation("User Service initialized");
        }

        public IAccount GetUser() {
            Console.Write("Provide the password of your keystore: ");
            string password = Console.ReadLine();

            try {
                return _keyStoreService.GetAccount(password);
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

        public IAccount RegisterAccount() {
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
            return _keyStoreService.GetAccount(password);
        }

        public IAccount RegisterUsername() {
            throw new System.NotImplementedException();
        }
    }
}

using Console_Application.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Nethereum.RPC.Accounts;

namespace Console_Application.Services.UserService {
    public class UserService : IUserService {
        private readonly IKeyStoreService _keyStoreService;
        private readonly ILogger<Program> _logger;

        public UserService(IKeyStoreService keyStore, ILogger<Program> logger) {
            _keyStoreService = keyStore;
            _logger = logger;
            _logger.LogInformation("User Service initialized");
        }

        public IAccount GetUser(string password) {
            throw new System.NotImplementedException();
        }

        public IAccount RegisterAccount(string password, string pk) {
            throw new System.NotImplementedException();
        }

        public IAccount RegisterUsername(string username) {
            throw new System.NotImplementedException();
        }
    }
}

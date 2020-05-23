using Console_Application.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Nethereum.RPC.Accounts;
using Nethereum.Web3;
using System;
using System.Threading.Tasks;

namespace Console_Application.Services.LoginService {
    public class LoginService : ILoginService {
        private readonly IUserService _userService;
        private readonly ILogger<Program> _logger;

        public LoginService(IUserService userService, ILogger<Program> logger) {
            _userService = userService;
            _logger = logger;
        }

        public Web3 Login() {
            if (!_userService.HasAccount()) {
                Console.WriteLine("We could't find a keyfile for your account");
                Web3 account = _userService.RegisterAccount();
                return account;
            }

            return _userService.GetUser();
        }

        public async Task SayGreeting() {
            if (!(await _userService.HasUsername())){
                _logger.LogWarning("No username found, Registering username");
                await _userService.RegisterUsername();
            }
        }
    }
}

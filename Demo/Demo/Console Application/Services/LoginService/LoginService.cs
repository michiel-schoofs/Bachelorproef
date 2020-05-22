using Console_Application.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Nethereum.RPC.Accounts;
using System;

namespace Console_Application.Services.LoginService {
    public class LoginService : ILoginService {
        private readonly IUserService _userService;
        private readonly ILogger<Program> _logger;

        public LoginService(IUserService userService, ILogger<Program> logger) {
            _userService = userService;
            _logger = logger;
        }

        public IAccount login() {
            if (!_userService.HasAccount()) {
                Console.WriteLine("We could't find a keyfile for your account");
                IAccount account = _userService.RegisterAccount();
                return account;
            }

            return _userService.GetUser();
        }
    }
}

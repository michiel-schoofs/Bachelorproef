using Nethereum.Model;
using Nethereum.Web3.Accounts;
using Nethereum.RPC.Accounts;
using Account = Nethereum.Web3.Accounts.Account;
using Nethereum.Web3;
using System;
using Microsoft.Extensions.DependencyInjection;
using Console_Application.Services.Interfaces;
using Console_Application.Services.KeyStore;
using Console_Application.Services.UserService;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging.Configuration;
using Console_Application.Services.LoginService;
using Figgle;

namespace Console_Application {
    public class Program {
        static void Main(string[] args) {
            Console.WriteLine(FiggleFonts.Isometric1.Render("Git Eth"));

            ServiceProvider provider = BuildProvider();
            ILogger logger = GetLogger(provider);
            logger.LogInformation("Application started...");

            ILoginService loginService = provider.GetService<ILoginService>();
            IAccount account = loginService.login();

            Web3 web3 = new Web3(account, url: "http://127.0.0.1:7545");
            string[] accounts = web3.Eth.Accounts.SendRequestAsync().Result;
        }

        public static ServiceProvider BuildProvider() {
            return new ServiceCollection()
                .AddSingleton<IKeyStoreService, KeyStoreService>()
                .AddSingleton<IUserService, UserService>()
                .AddSingleton<ILoginService,LoginService>()
                .AddLogging(opt => {
                    opt.AddConsole(c => {
                        c.DisableColors = false;
                        c.LogToStandardErrorThreshold = LogLevel.Trace;
                    });
                })
                .BuildServiceProvider();
        }

        public static ILogger GetLogger(ServiceProvider provider) {
            return provider.GetService<ILoggerFactory>().CreateLogger<Program>();
        }

    }
}

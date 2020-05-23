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
using Console_Application.Services.ContractService;
using System.IO;

namespace Console_Application {
    public class Program {
        static void Main(string[] args) {
            Console.WriteLine(FiggleFonts.Isometric1.Render("Git Eth"));
            ServiceProvider provider = BuildProvider();

            ILogger logger = GetLogger(provider);
            logger.LogInformation("Application started...");

            ILoginService loginService = provider.GetService<ILoginService>();
            Web3 web3 = loginService.Login();

            IContractService contractService = provider.GetService<IContractService>();

            //Production
            //contractService.DeployContracts(web3, Directory.GetCurrentDirectory() + "/DeployedContracts.json").Wait();
            //Development
            string directory = Directory.GetCurrentDirectory();
            string projectDirectory = Directory.GetParent(directory).Parent.Parent.FullName + "/DeployedContracts.json";
            contractService.DeployContracts(web3, projectDirectory).Wait();

            loginService.SayGreeting().Wait();
        }

        public static ServiceProvider BuildProvider() {
            ServiceCollection collection = new ServiceCollection();
            collection.AddSingleton<IKeyStoreService, KeyStoreService>()
                .AddSingleton<IUserService, UserService>()
                .AddSingleton<ILoginService, LoginService>()
                .AddLogging(opt => {
                    opt.AddConsole(c => {
                        c.DisableColors = false;
                        c.LogToStandardErrorThreshold = LogLevel.Trace;
                    });
                })
                .AddSingleton<IContractService>(s => new ContractService(
                   s.GetService<ILoggerFactory>(), 
                   Directory.GetCurrentDirectory() + "\\CompiledContracts.json"
                ));

            return collection.BuildServiceProvider();
        }

        public static ILogger GetLogger(ServiceProvider provider) {
            return provider.GetService<ILoggerFactory>().CreateLogger<Program>();
        }

    }
}

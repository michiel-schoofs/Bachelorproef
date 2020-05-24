using Console_Application.Contracts.Contract;
using Console_Application.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Console_Application.Services.RepositoryService {
    public class RepositoryService : IRepositoryService {
        private readonly ILogger<Program> _logger;
        private readonly IUserService _userService;
        private readonly IContractService _contractService;

        public RepositoryService(ILogger<Program> logger, IUserService userService,
            IContractService contractService) {
            _logger = logger;
            _userService = userService;
            _contractService = contractService;
        }

        public async Task<string[]> AddRepository(string name) {
            throw new System.NotImplementedException();
        }

        public async Task<string[]> GetRepositories() {
            throw new System.NotImplementedException();
        }

        public async Task GetRepositoriesByCurrentUser() {
            try {
                string username = await _userService.GetUsername();
                await GetRepositoriesByUser(username);
            } catch (Exception e) {
                _logger.LogError("Something went wrong with the GetRepositoriesByUserFunction {0}", e.Message);
                Console.WriteLine("Something went wrong with the execution of this function");
                Console.Beep();
            }
        }

        public async Task GetRepositoriesByUser(string username) {
            try {
                _logger.LogInformation("Getting repositories for user {0}", username);

                Web3 client = _userService.GetUser();

                ReturnRepositoriesByUserFunction getRepoFunc = new ReturnRepositoriesByUserFunction() {
                    Username = username
                };

                UsernameExists ue = new UsernameExists() {
                    Username = username
                };

                string addressuserservice = _contractService.GetAddressDeployedContract("UserService");

                var uehandler = client.Eth.GetContractQueryHandler<UsernameExists>();
                bool exist = await uehandler.QueryAsync<bool>(addressuserservice,ue);

                var handler = client.Eth.GetContractQueryHandler<ReturnRepositoriesByUserFunction>();
                string repos = await handler.QueryAsync<string>(addressuserservice, getRepoFunc);
                
                if (repos.Length == 0) {
                    Console.WriteLine("There are no repos for the user: " + username);
                    return;
                } else {
                    /*foreach (string repoad in repos) {
                        GetNameOfRepo getNameFunction = new GetNameOfRepo();
                        GetIdOfRepo getIdFunction = new GetIdOfRepo();

                        var namehand = client.Eth.GetContractQueryHandler<GetNameOfRepo>();
                        var idhand = client.Eth.GetContractQueryHandler<GetIdOfRepo>();

                        string name = await namehand.QueryAsync<string>(repoad,getNameFunction);
                        int id = await idhand.QueryAsync<int>(repoad,getIdFunction);
                        Console.WriteLine(string.Format($"id:{0} - name:{1}", id, name));
                    }*/
                }

            } catch (RpcResponseException e) {
                Web3 client = _userService.GetUser();
                _logger.LogError("Something went wrong with the GetRepositoriesByUserFunction {0}", e.Message);
                Console.WriteLine("Something went wrong with the execution of this function");
                Console.Beep();
            }
        }
    }
}

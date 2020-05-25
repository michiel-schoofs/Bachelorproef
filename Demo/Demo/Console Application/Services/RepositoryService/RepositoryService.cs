using Console_Application.Contracts.Contract;
using Console_Application.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Nethereum.Contracts;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Console_Application.Services.RepositoryService {
    public class RepositoryService : IRepositoryService {
        private readonly ILogger<Program> _logger;
        private readonly IUserService _userService;
        private readonly IContractService _contractService;
        private readonly IIPFSService _ipfsService;

        public RepositoryService(ILogger<Program> logger, IUserService userService,
            IContractService contractService,IIPFSService iPFSService) {
            _logger = logger;
            _userService = userService;
            _contractService = contractService;
            _ipfsService = iPFSService;
        }

        public async Task AddRepository(string name) {
            try {
                _logger.LogInformation("Attempting to add repository {0}", name);

                if (string.IsNullOrEmpty(name))
                    throw new ArgumentException("Please provide a valid name for the repository");

                if (!_contractService.ContractDeployed("RepositoryService"))
                    throw new Exception("The contract is not deployed");

                string contractAd = _contractService.GetAddressDeployedContract("RepositoryService");

                Web3 client = _userService.GetUser();
                CreateRepositoryFunction crf = new CreateRepositoryFunction() {
                    Name = name
                };

                var handler = client.Eth.GetContractTransactionHandler<CreateRepositoryFunction>();
                TransactionReceipt tr = await handler.SendRequestAndWaitForReceiptAsync(contractAd, crf);

                List<EventLog<RepositoryAddedEvent>> events = tr.DecodeAllEvents<RepositoryAddedEvent>();
                
                if (events.Count > 0) {
                    Console.WriteLine(string.Format("Repository {0} added", name));
                }
            } catch (Exception e) {
                _logger.LogError("Something went wrong with the AddRepositoryFunction {0}", e.Message);
                Console.WriteLine("Something went wrong with the execution of this function");
                Console.Beep();
            }
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

                ReturnUserFunction getRepoFunc = new ReturnUserFunction() {
                    Username = username
                };

                string addressuserservice = _contractService.GetAddressDeployedContract("UserService");

                var contractHandler = client.Eth.GetContractHandler(addressuserservice);
                ReturnUserOutputDTO user = await contractHandler.QueryDeserializingToObjectAsync<ReturnUserFunction, ReturnUserOutputDTO>(getRepoFunc);
                
                if (user.ReturnValue1.Repositories.Count == 0) {
                    Console.WriteLine("There are no repos for the user: " + username);
                    return;
                } else {
                    foreach (string repoad in user.ReturnValue1.Repositories) {
                        GetNameOfRepo getNameFunction = new GetNameOfRepo();
                        GetIdOfRepo getIdFunction = new GetIdOfRepo();

                        var namehand = client.Eth.GetContractQueryHandler<GetNameOfRepo>();
                        var idhand = client.Eth.GetContractQueryHandler<GetIdOfRepo>();

                        string name = await namehand.QueryAsync<string>(repoad,getNameFunction);
                        int id = await idhand.QueryAsync<int>(repoad,getIdFunction);
                        Console.WriteLine(string.Format("id:{0} - name:{1}", id, name));
                    }
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

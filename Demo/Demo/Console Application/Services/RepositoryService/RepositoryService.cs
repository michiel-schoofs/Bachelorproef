using Console_Application.Contracts.Contract;
using Console_Application.Services.Interfaces;
using Console_Application.Services.Models;
using Microsoft.Extensions.Logging;
using Nethereum.Contracts;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Console_Application.Services.RepositoryService {
    public class RepositoryService : IRepositoryService {
        private readonly ILogger<Program> _logger;
        private readonly IUserService _userService;
        private readonly IContractService _contractService;
        private readonly IIPFSService _ipfsService;
        private static string repoFile;

        public RepositoryService(ILogger<Program> logger, IUserService userService,
            IContractService contractService, IIPFSService iPFSService) {
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

                string path = _ipfsService.SelectPath();

                if (path == null) {
                    throw new Exception("Please select a valid folder");
                }

                string cid = await _ipfsService.AddDirectoryOnIPFS(path);

                string contractAd = _contractService.GetAddressDeployedContract("RepositoryService");

                Web3 client = _userService.GetUser();
                CreateRepositoryFunction crf = new CreateRepositoryFunction() {
                    Name = name,
                    Cid = cid
                };

                var handler = client.Eth.GetContractTransactionHandler<CreateRepositoryFunction>();
                TransactionReceipt tr = await handler.SendRequestAndWaitForReceiptAsync(contractAd, crf);

                List<EventLog<RepositoryAddedEvent>> events = tr.DecodeAllEvents<RepositoryAddedEvent>();

                if (events.Count > 0) {
                    Console.WriteLine(string.Format("Repository {0} added", name));
                }

                RepositorySerialized repos = new RepositorySerialized() {
                    CID = cid,
                    Name = name,
                    Path = path
                };

                WriteToRepositoryFile(repos);
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
                        GetCidOfRepo getCidFunction = new GetCidOfRepo();

                        var namehand = client.Eth.GetContractQueryHandler<GetNameOfRepo>();
                        var idhand = client.Eth.GetContractQueryHandler<GetIdOfRepo>();
                        var cidhand = client.Eth.GetContractQueryHandler<GetCidOfRepo>();

                        string name = await namehand.QueryAsync<string>(repoad, getNameFunction);
                        int id = await idhand.QueryAsync<int>(repoad, getIdFunction);
                        string cid = await cidhand.QueryAsync<string>(repoad, getCidFunction);

                        bool localExist = LocalRepositoryExist(name);
                        string status = localExist ? "local" : "remote";

                        if (localExist) {
                            bool update = cid.Equals(_ipfsService.GetCid(name));

                            FileStream stream = File.OpenRead(repoFile);

                            using (StreamReader reader = new StreamReader(stream)) {
                                string json = reader.ReadToEnd();
                                List<RepositorySerialized> repos = new List<RepositorySerialized>();

                                if (json.Replace("{", "").Replace("}", "").Trim().Length != 0)
                                    repos = JsonConvert.DeserializeObject<List<RepositorySerialized>>(json);

                                var repo = repos.First(r => r.Name.Equals(name));
                                string localCid = await _ipfsService.GetCid(repo.Path);
                                string notUpToDate = localCid.Equals(cid) ? "up-to-date" : "please pull new changes";

                                Console.WriteLine(string.Format("id:{0} - name:{1} - cid:{2} - status:{3} - clean? {4}"
                                    , id, name, cid, status,notUpToDate));
                            }

                        } else {
                            Console.WriteLine(string.Format("id:{0} - name:{1} - cid:{2} - status:{3}"
                                , id, name, cid, status));
                        }
                    }
                }

            } catch (RpcResponseException e) {
                Web3 client = _userService.GetUser();
                _logger.LogError("Something went wrong with the GetRepositoriesByUserFunction {0}", e.Message);
                Console.WriteLine("Something went wrong with the execution of this function");
                Console.Beep();
            }
        }

        public bool LocalRepositoryExist(string name) {
            try {
                if (!File.Exists(repoFile))
                    throw new Exception("Repository file not found");

                FileStream stream = File.OpenRead(repoFile);

                using (StreamReader reader = new StreamReader(stream)) {
                    string json = reader.ReadToEnd();
                    List<RepositorySerialized> repos = new List<RepositorySerialized>();

                    if (json.Replace("{", "").Replace("}", "").Trim().Length != 0)
                        repos = JsonConvert.DeserializeObject<List<RepositorySerialized>>(json);

                    return repos.Any(n => n.Name.Equals(name));
                }
            } catch (Exception e) {
                _logger.LogError("Something went wrong with the GetLocalRepository {0}", e.Message);
                Console.WriteLine("Something went wrong with the execution of this function");
                Console.Beep();
                return false;
            }
        }

        public void SetRepositoryFile(string path) {
            repoFile = path;
        }

        public void WriteToRepositoryFile(RepositorySerialized repoSer) {
            try {
                if (!File.Exists(repoFile))
                    throw new Exception("Repository file not found");

                FileStream readstream = File.OpenRead(repoFile);
                var repos = new List<RepositorySerialized>();

                using (StreamReader reader = new StreamReader(readstream)) {
                    string json = reader.ReadToEnd();
                    repos = JsonConvert.DeserializeObject<List<RepositorySerialized>>(json);

                    repos.Add(repoSer);
                }

                File.Delete(repoFile);
                File.WriteAllText(repoFile, JsonConvert.SerializeObject(repos));
            } catch (Exception e) {
                _logger.LogError("Something went wrong with the GetLocalRepository {0}", e.Message);
                Console.WriteLine("Something went wrong with the execution of this function");
                Console.Beep();
            }
        }

        public void CleanRepositoryFile() {
            try {
                if (!File.Exists(repoFile))
                    throw new Exception("Repository file not found");

                FileStream readstream = File.OpenRead(repoFile);
                List<RepositorySerialized> repos = new List<RepositorySerialized>();

                using (StreamReader reader = new StreamReader(readstream)) {
                    string json = reader.ReadToEnd();
                    repos = JsonConvert.DeserializeObject<List<RepositorySerialized>>(json);

                    foreach (RepositorySerialized repo in repos.ToArray()) {
                        if (!Directory.Exists(repo.Path))
                            repos.Remove(repo);
                    }
                }

                File.Delete(repoFile);
                File.WriteAllText(repoFile, JsonConvert.SerializeObject(repos));
            } catch (Exception e) {
                _logger.LogError("Something went wrong with the Cleaning the repository file {0}", e.Message);
                Console.WriteLine("Something went wrong with the execution of this function");
                Console.Beep();
            }
        }

        [STAThread]
        public async Task CloneRepository() {
            try {
                string name = "";

                do {
                    Console.Write("Please provide a name for the repository you want to clone: ");
                    name = Console.ReadLine();
                } while (string.IsNullOrEmpty(name));

                CheckIfRepoExistsFunction checkIfRepoExistsFunction = new CheckIfRepoExistsFunction() {
                    Name = name
                };

                if (!_contractService.ContractDeployed("RepositoryService"))
                    throw new Exception("Repository not deployed");

                string ad = _contractService.GetAddressDeployedContract("RepositoryService");

                Web3 user =_userService.GetUser();
                var handler = user.Eth.GetContractQueryHandler<CheckIfRepoExistsFunction>();
                
                bool exists = await handler.QueryAsync<bool>(ad, checkIfRepoExistsFunction);

                if (!exists)
                    throw new Exception("The specified repository doesn't exist");

                GetRepositoryFunction getRepoFunction = new GetRepositoryFunction() { 
                    Name = name
                };
                var getRepoHandler = user.Eth.GetContractQueryHandler<GetRepositoryFunction>();

                string contractAd = await getRepoHandler.QueryAsync<string>(ad, getRepoFunction);

                if (string.IsNullOrEmpty(contractAd))
                    throw new Exception("Something went wrong with the execution of this function");

                var cidHandler = user.Eth.GetContractQueryHandler<GetCidOfRepo>();
                string cid = await cidHandler.QueryAsync<string>(contractAd, new GetCidOfRepo());

                if (string.IsNullOrEmpty(cid))
                    throw new Exception("Something went wrong with the execution of this function");

                string path = $"C:\\" + name;
                await _ipfsService.GetDirectoryFromIPFS(path, cid);
                Console.WriteLine(string.Format("repository {0} succefully cloned to {1}", name,path));

                RepositorySerialized repos = new RepositorySerialized() {
                    Name = name,
                    CID = cid,
                    Path = path
                };

                WriteToRepositoryFile(repos);
            } catch (Exception e) {
                _logger.LogError("Something went wrong with the GetLocalRepository {0}", e.Message);
                Console.WriteLine(e.Message);
                Console.Beep();
            }
        }
    }
}

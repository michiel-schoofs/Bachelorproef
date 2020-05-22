using Console_Application.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Console_Application.Services.ContractService {
    public class ContractService : IContractService {
        private readonly ILogger<Program> _logger;
        private IDictionary<string, string> _contracts;

        public string FilePath { get; set; }

        public ContractService(ILogger<Program> logger) {
            Initialize();
            _logger = logger;
        }

        private void Initialize() {
            try {

                FileStream file = File.OpenRead(FilePath);
                
                using (StreamReader reader = new StreamReader(file)) {
                    string json = reader.ReadToEnd();
                    _contracts = JsonConvert.DeserializeObject<IDictionary<string, string>>(json);
                }

                file.Close();

            } catch (Exception e) {
                _logger.LogError(e.Message);
                Console.WriteLine("Something went wrong with the contracts parsing file");
                Console.Beep();
                System.Environment.Exit(1);
            }
        }

        public string ContractExist(string name) {
            throw new System.NotImplementedException();
        }

        public string GetByteCode(string name) {
            throw new System.NotImplementedException();
        }

        public string[] GetContractsNames() {
            throw new System.NotImplementedException();
        }
    }
}

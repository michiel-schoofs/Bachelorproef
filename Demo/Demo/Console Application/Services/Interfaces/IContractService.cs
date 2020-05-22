using System;
using System.Collections.Generic;
using System.Text;

namespace Console_Application.Services.Interfaces {
    public interface IContractService {
        public string GetByteCode(string name);
        public string[] GetContractsNames();
        public string ContractExist(string name);
    }
}

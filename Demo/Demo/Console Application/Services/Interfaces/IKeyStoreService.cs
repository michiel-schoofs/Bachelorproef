using Nethereum.Web3.Accounts;

namespace Console_Application.Services.Interfaces{
    public interface IKeyStoreService {
        public Account GetAccount(string password);
        public bool HasKeyStore();
        public void GenerateKeyStore(string password, byte[] pk,string address);
    }
}

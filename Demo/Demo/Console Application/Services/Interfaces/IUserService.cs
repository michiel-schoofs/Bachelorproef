using Nethereum.RPC.Accounts;

namespace Console_Application.Services.Interfaces {
    public interface IUserService {
        public IAccount GetUser(string password);
        public IAccount RegisterAccount(string password, string pk);
        public IAccount RegisterUsername(string username);
    }
}

using Nethereum.RPC.Accounts;

namespace Console_Application.Services.Interfaces {
    public interface IUserService {
        public IAccount GetUser();
        public IAccount RegisterAccount();
        public IAccount RegisterUsername();
        public bool HasAccount();
    }
}

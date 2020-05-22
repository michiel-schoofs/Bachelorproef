using Nethereum.Web3;
using System.Threading.Tasks;

namespace Console_Application.Services.Interfaces {
    public interface ILoginService {
        public Web3 Login();
        public Task SayGreeting();
    }
}

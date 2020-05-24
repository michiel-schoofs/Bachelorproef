using System.Threading.Tasks;

namespace Console_Application.Services.Interfaces {
    public interface IRepositoryService {
        public Task GetRepositoriesByUser(string username);
        public Task GetRepositoriesByCurrentUser();
        public Task<string[]> GetRepositories();
        public Task<string[]> AddRepository(string name);
    }
}

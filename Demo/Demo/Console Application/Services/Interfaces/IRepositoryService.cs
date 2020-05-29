using Console_Application.Services.Models;
using System.Threading.Tasks;

namespace Console_Application.Services.Interfaces {
    public interface IRepositoryService {
        public Task GetRepositoriesByUser(string username);
        public Task GetRepositoriesByCurrentUser();
        public Task AddRepository(string name);
        public bool LocalRepositoryExist(string name);
        public void SetRepositoryFile(string path);
        public void WriteToRepositoryFile(RepositorySerialized repoSer);
        public RepositorySerialized GetLocalRepository(string name);
        public Task CloneRepository();
        public void CleanRepositoryFile();
        public Task AddChangesAsync();
        public Task GetChanges();
        public Task GetEarlierVersion();
    }
}

using System.Threading.Tasks;

namespace Console_Application.Services.Interfaces {
    public interface IIPFSService {
        public string SelectPath();
        public Task<string> AddDirectoryOnIPFS(string path);
    }
}

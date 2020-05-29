using System.Threading.Tasks;

namespace Console_Application.Services.Interfaces {
    public interface IIPFSService {
        public void LaunchDaemon();
        public string SelectPath();
        public Task<string> AddDirectoryOnIPFS(string path);
        public Task GetDirectoryFromIPFS(string path, string cid);
        public Task<string> GetCid(string path);
    }
}

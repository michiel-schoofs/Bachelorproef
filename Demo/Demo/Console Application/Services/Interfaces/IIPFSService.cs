namespace Console_Application.Services.Interfaces {
    public interface IIPFSService {
        public string SelectPath();
        public string AddFileOnIPFS(string path);
    }
}

namespace Console_Application.Services.Interfaces{
    public interface IKeyStoreService {
        public string GetPrivateKey(string password);
        public bool HasKeyStore();
        public void GenerateKeyStore(string password, string pk);
    }
}

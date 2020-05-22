using Console_Application.Services.Interfaces;

namespace Console_Application.Services.KeyStore {
    public class KeyStoreService : IKeyStoreService {
        private readonly string directory = "./Files/";

        public void GenerateKeyStore(string password, string pk) {
            throw new System.NotImplementedException();
        }

        public string GetPrivateKey(string password) {
            throw new System.NotImplementedException();
        }

        public bool HasKeyStore() {
            throw new System.NotImplementedException();
        }
    }
}

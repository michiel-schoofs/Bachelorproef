using Console_Application.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Nethereum.KeyStore;
using Nethereum.KeyStore.Crypto;
using Nethereum.KeyStore.Model;
using Nethereum.Web3.Accounts;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Console_Application.Services.KeyStore {
    public class KeyStoreService : IKeyStoreService {
        private readonly ILogger<Program> _logger;
        private readonly string _dir = Directory.GetCurrentDirectory() + "/Files/";

        public KeyStoreService(ILogger<Program> logger) {
            _logger = logger;
        }

        public void GenerateKeyStore(string password, byte[] pk,string address) {
            var keyStoreService = new KeyStoreScryptService();
            var scryptParams = new ScryptParams { Dklen = 32, N = 262144, R = 1, P = 8 };

            KeyStore<ScryptParams> store = keyStoreService.EncryptAndGenerateKeyStore(
                password, pk, address,scryptParams
            );

            string json = keyStoreService.SerializeKeyStoreToJson(store);
            GenerateFile(json, $"KeyStore_{DateTime.Now.ToShortDateString().Replace("/","-")}.json");
        }

        private void GenerateFile(string content,string filename) {
            FileStream stream = File.Create(_dir+filename);

            using (StreamWriter writer = new StreamWriter(stream)) {
                writer.Write(content);
                writer.Flush();
            }

            stream.Close();
        }

        private void RemoveFilesFromDirectory(string[] files) {
                foreach (string file in files) {
                    _logger.LogWarning("errasing " + file);
                    File.Delete(file);
                }
        }

        public bool HasKeyStore() {
            try {
                string[] files = Directory.GetFiles(_dir);
                return files.Length != 0;
            } catch (DirectoryNotFoundException) {
                _logger.LogError("Directory Files not found, creating the directory manually");
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/Files/");
                return false;
            }
        }

        public Account GetAccount(string password) {
            string[] files =  Directory.GetFiles(_dir);
            if (files.Length > 1) {
                _logger.LogError("There are more then one file in the keystore directory");
                RemoveFilesFromDirectory(files);
            }

            if (files.Length == 0)
                return null;

            FileStream stream = File.OpenRead(files[0]);
            string result;

            using (StreamReader reader = new StreamReader(stream)) {
                 result = reader.ReadToEnd();
            }

            Account ac = Account.LoadFromKeyStore(result, password);

            stream.Close();

            return ac;
        }
    }
}

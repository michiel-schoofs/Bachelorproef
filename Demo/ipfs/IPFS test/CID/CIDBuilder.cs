using Ipfs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace IPFS_test.CID {
    /// <summary>
    /// Builds a V0 CID
    /// </summary>
    public class CIDBuilder {
        private readonly byte[] _file;
        private readonly byte _algorithm;

        public CIDBuilder(string path, byte algorithm) {
            CheckIfFileExists(path);
            _file = File.ReadAllBytes(path);
            _algorithm = algorithm;
        }

        private void CheckIfFileExists(string path) {
            if (!File.Exists(path)) {
                throw new FileLoadException("The specified path doesn't exist");
            }
        }

        public string BuildCID() {
            byte[] hash = HashContent();
            int hashlength = hash.Length;

            if (hashlength == 0) {
                throw new CryptographicException("Something went wrong with the hashing algorithm");
            }

            //Build v0 cid <identifier algorithm> <length of hash> <hash value>
            List<byte> cid = new List<byte>(){ _algorithm, Convert.ToByte(hashlength) };
            cid.AddRange(hash);

            //v0 encodes using base58btc
            return Base58.Encode(cid.ToArray());
        }

        public byte[] HashContent() {
            byte[] hash = new byte[0];

            if (_algorithm == HashingAlgorithm.SHA2_256) {
                hash = SHA256.Create().ComputeHash(_file);
            }

            return hash;
        }
    }
}

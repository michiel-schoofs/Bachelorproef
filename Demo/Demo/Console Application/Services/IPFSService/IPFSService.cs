using Console_Application.Services.Interfaces;
using DotImaging;
using System;

namespace Console_Application.Services {
    public class IPFSService : IIPFSService {
        public string AddFileOnIPFS(string path) {
            throw new System.NotImplementedException();
        }

        public string SelectPath() {
            string selectedFolder = UI.SelectFolder();
            return selectedFolder;
        }
    }
}

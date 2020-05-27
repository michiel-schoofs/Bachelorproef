using Console_Application.Services.Interfaces;
using Ipfs;
using Ipfs.CoreApi;
using Ipfs.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Console_Application.Services {
    public class IPFSService : IIPFSService {
        private static readonly IpfsClient ipfs = new IpfsClient();
        private readonly ILogger<Program> _logger;

        public IPFSService(ILogger<Program> logger) {
            _logger = logger;
        }

        public void LaunchDaemon() {
            _logger.LogInformation("Attempting to launch IPFS daemon");
            Process p = new Process();
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "cmd.exe";
            info.RedirectStandardInput = true;
            info.UseShellExecute = false;

            p.StartInfo = info;
            p.Start();


            using (StreamWriter sw = p.StandardInput) {
                if (sw.BaseStream.CanWrite) {
                    sw.WriteLine("ipfs daemon &");
                }
            }

            System.Threading.Thread.Sleep(4000);
            p.Close();
            _logger.LogInformation("IPFS daemon launched");
        }

        public async Task<string> AddDirectoryOnIPFS(string path) {
            IFileSystemNode node = await ipfs.FileSystem.AddDirectoryAsync(path, true);
            return node.Id.Encode();
        }

        public async Task GetDirectoryFromIPFS(string path, string cid) {
            IFileSystemNode node = await ipfs.FileSystem.ListFileAsync(cid);

            if (!node.IsDirectory)
                throw new Exception("This is not a directory");

            if (Directory.Exists(path))
                throw new Exception("A directory under the project name already exists");

            Directory.CreateDirectory(path);
            List<IFileSystemLink> directories = new List<IFileSystemLink>(node.Links);

            do {
                var first = directories.First();
                directories.Remove(first);
                node = await ipfs.FileSystem.ListFileAsync(first.Id);

                if (node.IsDirectory) {
                    directories.AddRange(node.Links);
                } else {
                    FileStream file = File.Create(path+"\\"+first.Name);
                    Stream s = await ipfs.FileSystem.ReadFileAsync(first.Id);
                     s.CopyTo(file);
                    file.Flush();
                    file.Close();
                }
            } while (directories.Count != 0);
        }

        public string SelectPath() {
            using (OpenFileDialog openFileDialog = new OpenFileDialog()) {
                openFileDialog.Title = "Select a folder to add";
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.ValidateNames = false;
                openFileDialog.CheckFileExists = false;
                openFileDialog.CheckPathExists = true;
                openFileDialog.FileName = "Folder";

                if (openFileDialog.ShowDialog() == DialogResult.OK) {
                    return Path.GetDirectoryName(openFileDialog.FileName);
                }

                return null;
            }
        }

        public async Task<string> GetCid(string path) {
            var options = new AddFileOptions { OnlyHash = true };
            var fsn = await ipfs.FileSystem.AddDirectoryAsync(path, true,options);
            return fsn.Id;
        }
    }
}

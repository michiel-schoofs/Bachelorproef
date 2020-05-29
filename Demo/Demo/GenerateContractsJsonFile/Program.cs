using GenerateContractsJsonFile.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;

/// <summary>
/// Throw away application to generate json file, only used for development purposes
/// </summary>
namespace GenerateContractsJsonFile {
    class Program {
        private static readonly IDictionary<string, string> contracts = new Dictionary<string, string>();
        private static readonly string filename = "C:\\Users\\gebruiker\\Desktop\\Studies\\Bachelor proef\\Bachelorproef\\Demo\\Demo\\Console Application\\CompiledContracts.json";
        private static readonly string contractsFolder = "C:\\Users\\gebruiker\\Desktop\\Studies\\Bachelor proef\\Bachelorproef\\Demo\\Demo\\Contracts\\";
        private static string BuildFolder { get{
                return contractsFolder + "build\\contracts\\";
            }
        }
        private static string[] files;


        static void Main(string[] args) {
            TruffleCompile();
            ReflectDirectory();
            PopulateDictonary();
            ConvertToFile();
        }

        static void ReflectDirectory() {
            files = Directory.GetFiles(BuildFolder);
        }

        static void PopulateDictonary() {
            foreach (string path in files) {
                FileStream stream = File.OpenRead(path);
                using (StreamReader reader = new StreamReader(stream)) {
                    Contract c = JsonConvert.DeserializeObject<Contract>(reader.ReadToEnd());

                    if (!c.bytecode.Equals("0x")) {
                        contracts.Add(c.contractName, c.bytecode);
                    }
                }

                stream.Close();
            }
        }

        static void TruffleCompile() {
            Process p = new Process();
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "cmd.exe";
            info.RedirectStandardInput = true;
            info.UseShellExecute = false;

            p.StartInfo = info;
            p.Start();


            using (StreamWriter sw = p.StandardInput) {
                if (sw.BaseStream.CanWrite) {
                    sw.WriteLine($"cd {contractsFolder}");
                    sw.WriteLine("truffle compile --all --network development");
                }
            }

            p.Close();
        }

        static void ConvertToFile() {
            if (File.Exists(filename))
                File.Delete(filename);

            FileStream stream = File.Create(filename);
            string json = JsonConvert.SerializeObject(contracts);

            using (StreamWriter writer = new StreamWriter(stream)) {
                writer.Write(json);
            }

            stream.Close();
        }
    }
}

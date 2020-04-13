using Ipfs;
using Ipfs.Http;
using System;
using System.Linq;
using IPFS_test.CID;

namespace IPFS_test {
    class Program {
        static void Main(string[] args) {
            //Makes a new IPFS node
            IpfsClient client = new IpfsClient();
            //add image to node
            IFileSystemNode node = client.FileSystem.AddFileAsync("images/index.jpg").Result;
            //show info about image
            Console.WriteLine($"The CID of this node is: {node.Id} and it contains {node.Links.Count()} links to other files");

            //Shows how CID are build
            CIDBuilder builder = new CIDBuilder("images/index.jpg",HashingAlgorithm.SHA2_256);
            Console.WriteLine("The generated CID is: " + builder.BuildCID());

            Console.ReadLine();
        }
    }
}

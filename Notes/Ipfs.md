# Ipfs

- To initiliase a node we use the command `peer init` -> We get a unique peer id also initialises a directory
-  IPFS  supports versioning on a P2P network. We can get the content from anyone who has the content.

- Content addressing as opposed to location based addressing. The advantage is that location addressing doesn't verify that the content we request is not gonna change http://sjqlkfsjdf.com/poodl.jpg is subject to change. Content adressing is immutable. We also have no more caching issues since the content is inherently immutable. 
- When storing an image: image -> Raw -> Hashfunction like Sha2-256 -> digest -> self describing hashing (CID) to future proof the hash function.
- CID : hashing algorithm code + length of digest + digest
- To add to ipfs we can use `ipfs add -r test` (-r = recursive) If it's a directory we see that we get two hashes the hashes of the directory itself and the files contained within

- Duplicate content gets stored on the same hash reducing the duplication

- We can use ipfs object get **cid** to get the object description

- Objects and file structures are represented with merkle trees (Directed Acrilic graphs -> No cycles)

  ![Merkle tree - Wikipedia](https://upload.wikimedia.org/wikipedia/commons/9/95/Hash_Tree.svg)

- If an object is too large (>262kb) object gets devided into multiple blocks and there's one base blocks that just references the individual blocks. You can view this property by using ipfs object get *cid* on the large file. An interesting property is that sub sections that are the same also get unduplicated.

- We can find where a specific block is stored using a hashtable. So if we have a specific cid we can query a hashtable to find the peer that has that block. This hash table is distributed over all peers and querried, IPFS uses a **Kademlia DHT**. So this is how we locate peers.

- Bitswap is used to get the data from the located peers. Bitswap is originated from Bit torrent. IPFS acts like a Giant swarm, seeding data to each other. Bitswap promotes data duplication to make the network robust against individual nodes leaving.   

- IPFS utilises file coin to incentivise users to share data over the network which is nothing more or less then a cryptocurrency.

- To identify a node ipfs also uses multibased adressing: /ipformat/ip/protocol/port/ipfs/CID

- Sensitive text document: We use private, public key encryption. Use PGP

## CID 

There are many hashing algorithms, we use sha2-256 in IPFS by default. Algorithms can break so a CID makes sure we can change the algorithm. That's why we use a self describing hash also called a multihash. Basic Multihash:

<algorithm number><hash-length><hash>

For version 0 the string representation is just a <IPLD -> dag-pb>base58btc(<multihash>)

However there are many ways to encode a file for example CBOR or JSON. To avoid these problems we  also include the encoding type in the CID. IPLD codex:

<dag-pb><algorithm number><hash-length><hash>

This leads to two versions of CID:

<version-number-cid><dag-pb><algorithm number><hash-length><hash>

However using a binary encoding to display the CID leads to a giant string of binary numbers so we need to encode the CID to a string but what encoding do we use to do that?

Multibase so the first symbol is the identifier for the encoding:

b... -> base 32 encoded

So to wrap it together:

<cid-version><ipld-format><multihash>

string representation:

<base>base =( <cid-version><ipld-format><multihash> )



## Commands

|          Command           |                         Description                          |                  Example                   |
| :------------------------: | :----------------------------------------------------------: | :----------------------------------------: |
|      ipfs add *name*       | Add a file to our local node. Returns the CID. The -w flag allows us to add metadata to our file (file name, size, hash) -> stored in a different location (so another cid) | ipfs add cat.jpg<br /> ipfs add -w cat.jpg |
|       ipfs cat *cid*       |                  Gets the content of a file                  |                  ipfs cat                  |
|        ipfs pin ls         | Gets all the files that are stored on your local node. To pin a piece of content just means to make it locally available. |                ipfs pin ls                 |
|       ipfs daemon &        |    Starts up the process and connects trough other peers     |               ipfs deamon &                |
|      ipfs swarm peers      |               All the peers we're connected to               |              ipfs swarm peers              |
|  ipfs dht findprovs <cid>  | Querry the Kademlia DHT directly asking for peers and returns their node id |          ipfs dht findprovs <cid>          |
| ipfs dht findpeer <nodeid> |           returns multi address to connect to peer           |         find dht findpeer <nodeid>         |

## IPNS

This associates a name with a specific piece of content. If the content gets changed over time people don't need to know the new CID of that content and can just do a lookup for the name you've provided using IPNS. 

- We can view all the files we've associated a name with the command `ipfs key list`
- We can generate a new key using `ipfs key gen --type=rsa test ` this makes a new key with the name test
- Next up we can publish a specific piece of content with a name attached to it. First we add it to the ipfs node: ipfs add capybara.jpg
- We associate a node with ipns by running the command `ipfs name publish --key=test QmUDxwFAhN9KLqjnLzkaXsDGASYVqGWvUwCV6Q9Y8wgvDE`

## Conclusion 

**[IPFS](http://ipfs.io) is a distributed file system that seeks to connect all computing devices with the same system of files.** In some ways, this is similar to the original aims of the Web, but IPFS is actually more similar to a single bittorrent swarm exchanging git  objects.  IPFS could become a new major subsystem of the internet. If  built right, it could complement or replace HTTP. It could complement or replace even more. It sounds crazy. It is crazy

*Bron ipfs readme*
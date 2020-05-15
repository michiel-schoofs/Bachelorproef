//The version of solidity
pragma solidity 0.5.0;

contract Meme {
    //We need a read and write function to store and read the hash
    //Create a state variable
    string memeHash;
    //Write function (can be called by anyone)
    function set(string memory _memeHash) public{
        memeHash = _memeHash;
    }
    //Read function (view)
    function get() public view returns (string memory){
        return memeHash;
    }
}
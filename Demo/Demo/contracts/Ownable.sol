pragma solidity >=0.6.0;

contract Ownable{
    address public owner;

    constructor() public{
        //Set the owner to the person that created the smart contract
        owner = msg.sender;
    }

    modifier onlyOwner(){
        require(msg.sender == owner,"Only the owner can execute this function");
        _;
    }

}
pragma solidity >=0.4.2;

contract Token {
    //State variable, available to the entire contract.
    //Write to blockchain every update
    //Already contains a getter
    uint256 public totalSupply;

    //Map the token balance
    //So we can look up the balance of a specific address
    mapping(address=>uint256) public balanceOf;

    //Constructor
    //Set tokens-total
    constructor(uint256 _initialSupply) public {
        totalSupply = _initialSupply;
        //Allocate initial supply so that's the person creating the smart contract
        balanceOf[msg.sender] = _initialSupply;
    }
}
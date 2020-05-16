pragma solidity >=0.4.2;

contract Token {
    //State variable, available to the entire contract.
    //Write to blockchain every update
    //Already contains a getter
    uint256 public totalSupply;

    //Constructor
    //Set tokens-total
    constructor() public {
        totalSupply = 1000000;
    }
}
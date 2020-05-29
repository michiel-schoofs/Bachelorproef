pragma solidity >=0.4.2;

contract Token {
    //State variable, available to the entire contract.
    //Write to blockchain every update
    //Already contains a getter
    uint256 public totalSupply;

    //Map the token balance
    //So we can look up the balance of a specific address
    mapping(address=>uint256) public balanceOf;

    //Name of our token
    string public name = 'Yellow Existential Crisis coin';
    string public symbol = 'YEC';
    string public standard = 'v1.0';

    //Constructor
    //Set tokens-total
    constructor(uint256 _initialSupply) public {
        totalSupply = _initialSupply;
        //Allocate initial supply so that's the person creating the smart contract
        balanceOf[msg.sender] = _initialSupply;
    }

    //Transfer
    function transfer(address _to, uint256 _value) public  returns (bool success) {
        //Trigger transfer event
        //Trigger exception if the account doesn't have enough
        //Return boolean
        require(balanceOf[msg.sender] >= _value,"Reverting transaction");
        balanceOf[msg.sender] -= _value;
        balanceOf[_to] += _value;
        return true;
    }
}
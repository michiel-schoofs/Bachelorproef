pragma solidity >=0.5.1;

//Contract to show how inheritance works
contract ERC20Token{
    string public name;
    mapping(address=> uint256) balance;

    constructor(string memory _name) public{
        name = _name;
    }

    //Make sure we can override the method with the virtual modifier
    function mint() public virtual {
        //The sender is a contract so we can't use msg.sender
        //We can use tx.origin to get the person initiaing a transaction
        balance[msg.sender]++;
    }
}


contract InheritanceContract  is ERC20Token {
    //You can't override state variabels
    //string override name = "MyToken";
    //You can have your own state variables inside of subclass
    string public symbol;
    address[] public owners;
    uint256 public ownerCount;

    //Super constructor call
    constructor(string memory _name, string memory _symbol) public ERC20Token(_name) {
        //Set property from super class
        symbol = _symbol;
    }

    //override function mint
    function mint() public override {
        super.mint();
        ownerCount++;
        owners.push(msg.sender);
    }
}
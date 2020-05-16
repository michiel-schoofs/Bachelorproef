pragma solidity >=0.5.1;

//Contract to show how we can talk to another contract.
contract ERC20Token{
    string public name;
    mapping(address=> uint256) balance;

    function mint() public {
        //The sender is a contract so we can't use msg.sender
        //We can use tx.origin to get the person initiaing a transaction
        balance[tx.origin]++;
    }
}


contract AdvancedConcepts {
    //Payable makes sure that the wallet is valid and can recieve ether
    address payable wallet;
    address public tokenContract;

    //Build in subscriber pattern, indexed fields can be filtered on when subscribing
    event SomeonePayed(
        address indexed _buyer,
        uint amount
    );

    constructor(address payable _owner, address _token) public {
        wallet = _owner;
        tokenContract = _token;
    }

    //default function that gets executed when calling this command also reffered to as a fallback function
    //When the fallback function is payable then use the recieve keyword if not you can use the fallback keyword
    receive() external payable {
        sendEther();
    }

    //A way to send ether to the wallet, payable makes sure we can actually attach ether to this function
    function sendEther() public payable {
        //transfer the atached amount of ehter to the wallet
        wallet.transfer(msg.value);
        //Gets pushed on the log, transactions are async and we can use this to listen effictively
        // We can also use this to know when a contract function got executed
        emit SomeonePayed(msg.sender, msg.value);
        //Get a reference to the contract and tell what it is
        ERC20Token _token = ERC20Token(address(tokenContract));
        //Call a function
        _token.mint();
    }
}
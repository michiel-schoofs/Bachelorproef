pragma solidity >=0.5.1;

contract AdvancedConcepts {
    //Payable makes sure that the wallet is valid and can recieve ether
    address payable wallet;
    //Build in subscriber pattern, indexed fields can be filtered on when subscribing
    event SomeonePayed(
        address indexed _buyer,
        uint amount
    );

    constructor(address payable _owner) public {
        wallet = _owner;
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
    }
}
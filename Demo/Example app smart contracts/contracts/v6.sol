pragma solidity >=0.6.0;
//This file is to demonstrate the major new changes
//of the latest version of solidity when writing this Thesis.
//Do note that we don't go indept into inline assembly updates that would take us too far.
contract v6 {
    //Functions can only be overriden when using the virtual keyword
    function thisFunctionIsOverridable() public pure virtual returns(uint) {
        return 0;
    }

    //In previous versions of solidity you could manipulate the length of an array directly.
    //This is no long possible instead of that use push and pop works like a LIFO principle
    uint[] public demoArray;

    function thisIsTheWayToManipulateArrays(uint val) public {
        //Previously this returned the new length of the array in the new version of solidity this is not the case.
        demoArray.push(val);
        demoArray.pop();
    }

    //Shadowing variables is not allowed, a state variable declared cannot be overriden
    uint public shadow = 15;

    //The fallback function is now devided into two parts: 
    //- The function when you call this contract with no function (implicitly payable) is called the recieve function
    //- The function when no other function matches the specified call this is now the fallback function

    //Set up to demonstrate the meaning of recieve function
    address payable public owner;

    constructor(address payable _ad) public {
        owner = _ad;
    }

}


contract v6_child is v6 {
    //Override functions need to contain the override keyword
    function thisFunctionIsOverridable() public override pure  returns(uint) {
        return 1;
    }

    //This is no longer possible
    //uint public shadow = 16

    //We need to specify a constructor this is due the setup fase of the new fallback function
    constructor(address payable _ad) public v6(_ad){}
}


//abstract contracts are a thing they differ from interfaces in that they can provide some function implementations.
abstract contract v6_abstract{
    function thisIsStillImplemented() public pure returns(uint){
        return 0;
    }

    function thisIsAnAbstractFunction() public virtual pure returns(uint){}
}

//Libraries need to implement all functions 
library Demo_Lib {
    function implementation() public pure returns (uint) {
        return 0;
    }
}
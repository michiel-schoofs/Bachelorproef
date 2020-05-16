pragma solidity >=0.6.0;
//This file is to demonstrate the major new changes
//of the latest version of solidity when writing this Thesis.
contract v6 {
    //Functions can only be overriden when using the virtual keyword
    function thisFunctionIsOverridable() public pure virtual returns(uint) {
        return 0;
    }

    //In previous versions of solidity you could manipulate the length of an array directly.
    //This is no long possible instead of that use push and pop works like a LIFO principle
    uint[] public demoArray;

    function thisIsTheWayToManipulateArrays(uint val) public {
        demoArray.push(val);
        demoArray.pop();
    }

    //Shadowing variables is not allowed, a state variable declared cannot be overriden
    uint public shadow = 15;
}


contract v6_child is v6 {
    //Override functions need to contain the override keyword
    function thisFunctionIsOverridable() public override pure  returns(uint) {
        return 1;
    }

    //This is no longer possible
    //uint public shadow = 16
}


//abstract contracts are a thing they differ from interfaces in that they can provide some function implementations.
abstract contract v6_abstract{
    function thisIsStillImplemented() public pure returns(uint){
        return 0;
    }

    function thisIsAnAbstractFunction() public virtual pure returns(uint){}
}
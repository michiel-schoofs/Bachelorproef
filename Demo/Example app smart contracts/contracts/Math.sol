pragma solidity >=0.5.0;

//Library
//Promote code reuse
library Math {
    //A library just stores a set of functions that we can reuse so we only have to maintain the code in one place
    //A pure function basically just tells we're only using the variables passed into the function and no state variables
    //Good practice to make this public
    function devide(uint _val1, uint _val2) public pure returns (uint){
        require(_val2 > 0,"");
        return _val1/_val2;
    }
}
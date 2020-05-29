pragma solidity >=0.5.0;
import './Math.sol';

contract UsingMath {
    uint256 public value;

    function calculate(uint _val1, uint _val2) public pure returns (uint) {
        //call the library
        return Math.devide(_val1,_val2);
    }
}
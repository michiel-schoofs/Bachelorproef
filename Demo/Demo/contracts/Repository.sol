pragma solidity >=0.6.0;
import "./Ownable.sol";
import './File.sol';

contract Repository is Ownable {
    File[] public files;
    
}
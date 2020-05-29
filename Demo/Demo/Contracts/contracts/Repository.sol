// SPDX-License-Identifier: MIT
pragma solidity >=0.6.0;
import "./Helpers/OwnableOtherContract.sol";

contract Repository is OwnableOtherContract {
    string public cid;
    string public name;
    uint256 public id;
    uint256 public version;

    mapping(uint256 => string) history;
    event CIDChanged(string _cid);

    constructor() public {
        version = 1;
    }

    function getName() public view returns(string memory) {
        return name;
    }

    function getVersion(uint256 indx) public view returns (string memory){
        return history[indx];
    }

    function getVersionCount() public view returns(uint256){
        return version;
    }

    function setCid(string memory _cid) public{
        require(bytes(_cid).length != 0,"Please provide a cid for this repository");
        history[version] = _cid;
        cid = _cid;
        emit CIDChanged(_cid);
        version++;
    }

    function getCid() public view returns(string memory){
        return cid;
    }

    function getId() public view returns(uint256) {
        return id;
    }

    function setName(string memory _name) public {
        require(bytes(_name).length != 0,"Please provide a name for the repository");
        name = _name;
    }

    function setId(uint256 _id) public {
        id = _id;
    }
}
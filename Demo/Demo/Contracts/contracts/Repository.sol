// SPDX-License-Identifier: MIT
pragma solidity >=0.6.0;
import "../node_modules/@openzeppelin/contracts/access/Ownable.sol";

contract Repository is Ownable{
    string public cid;
    string public name;
    uint256 public id;

    enum Visibility {
        pub,
        priv
    }

    Visibility public repoVisibility;

    constructor(string memory _name, uint256 _id) public {
        name = _name;
        id = _id;
    }
}
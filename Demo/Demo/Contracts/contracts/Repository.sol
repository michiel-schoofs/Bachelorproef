// SPDX-License-Identifier: MIT
pragma solidity >=0.6.0;
import "./Helpers/OwnableOtherContract.sol";

contract Repository is OwnableOtherContract {
    string public cid;
    string public name;
    uint256 public id;

    enum Visibility {
        pub,
        priv
    }

    Visibility public repoVisibility;

    function setName(string memory _name) public {
        require(bytes(_name).length != 0,"Please provide a name for the repository");
        name = _name;
    }

    function setId(uint256 _id) public {
        id = _id;
    }
}
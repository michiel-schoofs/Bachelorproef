// SPDX-License-Identifier: MIT
pragma solidity ^0.6.0;
import "../node_modules/@openzeppelin/contracts/utils/EnumerableSet.sol";


contract UserService {
    using EnumerableSet for EnumerableSet.AddressSet;

    struct User {
        address _address;
    }

    mapping(string => User) usernameToUser;
    EnumerableSet.AddressSet users;

    event NewUserAdded(string _username);
    event UserDeleted(string _username);

    function usernameExists(string memory _username) public view returns(bool) {
        return usernameToUser[_username]._address != address(0);
    }

    function userHasAccount() public view returns(bool){
        return users.contains(msg.sender);
    }

    function addUser(string memory _username) public {
        require(!usernameExists(_username),"You have to provide a unique username");
        require(!users.contains(msg.sender),"This user already has an account");

        usernameToUser[_username]._address = msg.sender;
        users.add(msg.sender);

        emit NewUserAdded(_username);
    }

    function getUser(string memory _username) public view returns(address){
        require(usernameExists(_username),"This user doesn't exist");
        return usernameToUser[_username]._address;
    }

    function getAmountOfUsers() public view returns(uint) {
        return users.length();
    }

    function removeUser(string memory _username) public {
        require(usernameExists(_username),"This user doesn't exist");

        address ad = usernameToUser[_username]._address;
        require(ad == msg.sender,"You can only delete your own account");

        users.remove(msg.sender);
        usernameToUser[_username]._address = address(0);

        emit UserDeleted(_username);
    }
}
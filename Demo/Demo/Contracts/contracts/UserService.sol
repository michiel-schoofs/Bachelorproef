// SPDX-License-Identifier: MIT
pragma solidity ^0.6.0;
pragma experimental ABIEncoderV2;

import "../node_modules/@openzeppelin/contracts/utils/EnumerableSet.sol";
import './Repository.sol';

contract UserService {
    using EnumerableSet for EnumerableSet.AddressSet;

    struct User {
        address _address;
        address[] _repositories;
    }

    mapping(string => User) usernameToUser;
    mapping(address => string) addressToUsername;

    EnumerableSet.AddressSet users;

    event NewUserAdded(string _username);
    event UserDeleted(string _username);
    event UserHasNewRepository(address repo);

    function ReturnUser(string memory _username) public view returns(User memory) {
        require(usernameExists(_username),"The specified user doesn't exist");
        User storage user = usernameToUser[_username];
        return user;
    }

    function usernameExists(string memory _username) public view returns(bool) {
        return usernameToUser[_username]._address != address(0);
    }

    function RequireTest() public pure {
        require(1==2,"Require test failed");
    }

    function GetUsernameFromTXUser() public view returns(string memory) {
        require(TXUserHasAccount(),"This user has no account");
        return addressToUsername[tx.origin];
    }

    function GetUsernameFromUser() public view returns(string memory) {
        require(userHasAccount(),"This user has no account");
        return addressToUsername[msg.sender];
    }

    function TXUserHasAccount() public view returns(bool){
        return users.contains(tx.origin);
    }

    function userHasAccount() public view returns(bool){
        return users.contains(msg.sender);
    }

    function addUser(string memory _username) public{
        require(!usernameExists(_username),"You have to provide a unique username");
        require(!users.contains(msg.sender),"This user already has an account");

        usernameToUser[_username]._address = msg.sender;
        addressToUsername[msg.sender] = _username;
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
        addressToUsername[msg.sender] = "";

        emit UserDeleted(_username);
    }

    function addRepositoryToUser(string memory _username, address _repoAd) public {
        require(usernameExists(_username),"The specified user doesn't exist");
        require(_repoAd != address(0),"You can't give an empty repository to a user");

        User storage user = usernameToUser[_username];
        user._repositories.push(_repoAd);

        emit UserHasNewRepository(_repoAd);
    }
}
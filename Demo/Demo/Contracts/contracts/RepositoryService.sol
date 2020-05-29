// SPDX-License-Identifier: MIT
pragma solidity >=0.6.0;
pragma experimental ABIEncoderV2;

import './Repository.sol';
import "../node_modules/@openzeppelin/contracts/utils/EnumerableSet.sol";
import './UserService.sol';

contract RepositoryService {
    using EnumerableSet for EnumerableSet.UintSet;
    uint256 public currentId;
    EnumerableSet.UintSet repoids;
    mapping(uint256 => address) idToRepository;

    event RepositoryAdded(uint256 id);

    mapping(string => address) nameToRepository;
    string[] names;

    UserService userServiceContract;

    constructor(address _UserService) public {
        currentId = 1;
        userServiceContract = UserService(_UserService);
    }

    //May want to change this to n-Names
    function GetNames() public view returns(string[] memory) {
        return names;
    }

    function CreateRepository(string memory name, string memory cid) public {
        require(!CheckIfRepoExists(name),"This name is already in use");

        Repository repo = new Repository();
        repo.setName(name);
        repo.setId(currentId);
        repo.setCid(cid);

        nameToRepository[name] = address(repo);
        idToRepository[currentId] = address(repo);

        repoids.add(currentId);
        names.push(name);

        emit RepositoryAdded(currentId);

        require(userServiceContract.TXUserHasAccount(),"You don't have an account");

        string memory username = userServiceContract.GetUsernameFromTXUser();
        userServiceContract.addRepositoryToUser(username,address(repo));

        currentId++;
    }

    function getRepository(uint256 id) public view returns(address) {
        require(CheckIfRepoExists(id),"This repository doesn't exist");
        return idToRepository[id];
    }

    function getRepository(string memory name) public view returns(address) {
        require(CheckIfRepoExists(name),"This repository doesn't exist");
        return nameToRepository[name];
    }

    function CheckIfRepoExists(uint256 id) public view returns(bool) {
        return repoids.contains(id);
    }

    function CheckIfRepoExists(string memory name) public view returns(bool) {
        return nameToRepository[name] != address(0);
    }
}
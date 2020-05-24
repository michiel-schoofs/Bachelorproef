// SPDX-License-Identifier: MIT
pragma solidity >=0.6.0;

import './Repository.sol';
import "../node_modules/@openzeppelin/contracts/utils/EnumerableSet.sol";

contract RepositoryService {
    using EnumerableSet for EnumerableSet.UintSet;
    uint256 public currentId;
    EnumerableSet.UintSet repoids;
    mapping(uint256 => Repository) idToRepository;

    event RepositoryAdded(uint256 id);

    mapping(string => Repository) nameToRepository;
    string[] names;

    constructor() public {
        currentId = 1;
    }

    function CreateRepository(string memory name) public {
        require(bytes(name).length != 0,"Please provide a name for the repository");
        require(!CheckIfRepoExists(name),"This name is already in use");

        Repository repo = new Repository(name, currentId);

        nameToRepository[name] = repo;
        idToRepository[currentId] = repo;

        repoids.add(currentId);
        names.push(name);

        emit RepositoryAdded(currentId);
        currentId++;
    }

    function CheckIfRepoExists(uint256 id) public view returns(bool) {
        return repoids.contains(id);
    }

    function CheckIfRepoExists(string memory name) public view returns(bool) {
        return nameToRepository[name].id() != 0;
    }
}
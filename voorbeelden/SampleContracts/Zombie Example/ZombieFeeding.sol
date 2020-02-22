pragma solidity >=0.5 <=0.6.1;

import './ZombieFactory.sol';

contract ZombieFeeding is ZombieFactory{
    KittyInterface kittyContract;

    function setKittyContractAddress(address _address) external onlyOwner {
        kittyContract = KittyInterface(_address);
    }

    function _triggerCooldown(Zombie storage _zombie) internal {
    _zombie.readyTime = uint32(now + cooldownTime);
    }

    function _isReady(Zombie storage _zombie) internal view returns (bool) {
        return (_zombie.readyTime <= now);
    }

    function feedOnKitty(uint _zombieId, uint _kittyId) public{
        uint kittyDna;
        (,,,,,,,,,kittyDna) = kittyContract.getKitty(_kittyId);
        feedAndMultiply(_zombieId, kittyDna,"kitty");
    }

    function feedAndMultiply(uint _zombieId, uint _targetDna, string memory _species) internal {
        require (msg.sender == zombieToOwner[_zombieId],"You can't feed someone elses zombies");
        Zombie storage myZombie = zombies[_zombieId];

        require(_isReady(myZombie),"This zombie isn't ready for feeding");

        uint _targetDnaNorm = _targetDna % dnaModulus;
        uint newDna = (_targetDnaNorm + myZombie.dna)/2;

        if(keccak256(abi.encodePacked(_species))==keccak256(abi.encodePacked("kitty"))){
            newDna = (newDna - (newDna % 100)) + 99;
        }

        _createZombie("No Name",newDna);
        _triggerCooldown(myZombie);
    }
}

interface KittyInterface{
    function getKitty(uint256 _id) external view returns (
    bool isGestating,
    bool isReady,
    uint256 cooldownIndex,
    uint256 nextActionAt,
    uint256 siringWithId,
    uint256 birthTime,
    uint256 matronId,
    uint256 sireId,
    uint256 generation,
    uint256 genes
    );
}
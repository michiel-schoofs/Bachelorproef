pragma solidity >=0.5.0 <=0.6.1;

import "./ZombieAttack.sol";
import "./ERC721.sol";

contract ZombieOwnable is ZombieAttack , ERC721 {
  mapping (uint => address) zombieApprovals;

  function balanceOf(address _owner) external override view returns (uint256) {
    return ownerZombieCount[_owner];
  }

  function ownerOf(uint256 _tokenId) external override view returns (address) {
    return zombieToOwner[_tokenId];
  }

  function _transfer(address _from,address _to, uint256 _tokenId) private {
    ownerZombieCount[_to]++;
    ownerZombieCount[_from]--;
    zombieToOwner[_tokenId] = _to;
    emit Transfer(_from, _to, _tokenId);
  }

  function transferFrom(address _from, address _to, uint256 _tokenId) external override payable {
    require(zombieToOwner[_tokenId] == msg.sender || zombieApprovals[_tokenId] == msg.sender, "you're not authorized to fufill this request");
    _transfer(_from,_to,_tokenId);
  }

  function approve(address _approved, uint256 _tokenId) external override payable onlyOwnerOf(_tokenId) {
    zombieApprovals[_tokenId] = _approved;
  }
}
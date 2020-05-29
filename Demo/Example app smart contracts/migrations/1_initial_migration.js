const Migrations = artifacts.require("./Migrations.sol");

//Migrations => Pushing a contract to the blockchain
module.exports= function(deployer) {
  deployer.deploy(Migrations);
};

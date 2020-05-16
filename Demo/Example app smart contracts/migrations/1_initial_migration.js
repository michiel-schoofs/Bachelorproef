const Migrations = artifacts.require("Migrations");

//Migrations => Pushing a contract to the blockchain
module.exports= function(deployer) {
  deployer.deploy(Migrations);
};

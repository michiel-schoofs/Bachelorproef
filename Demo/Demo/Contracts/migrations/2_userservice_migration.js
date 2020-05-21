const Migrations = artifacts.require("UserService");

module.exports = function(deployer) {
    deployer.deploy(Migrations);
  };
  
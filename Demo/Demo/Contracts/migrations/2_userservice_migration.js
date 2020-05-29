const Migrations = artifacts.require("UserService");
const RepositoryService = artifacts.require("RepositoryService");

module.exports = function(deployer) {
    deployer.deploy(Migrations).then(()=>{
        deployer.deploy(RepositoryService,Migrations.address);
    });
};
  
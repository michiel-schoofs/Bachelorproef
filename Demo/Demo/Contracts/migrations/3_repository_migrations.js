const RepositoryService = artifacts.require("RepositoryService");
const Repository = artifacts.require("Repository");

module.exports = function(deployer) {
    deployer.deploy(Repository);
    deployer.deploy(RepositoryService);
};
  
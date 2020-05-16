const Meme = artifacts.require("Meme");
//Deploys artifact, this is a json file generated by truffel inside of the ABI folder
//We can run this migration trough the truffle migrate command.
//We can interact with smart contracts trough the truffle console command
//const meme = await Meme.deployed(); -> Get instance of smart contract
//Now we can just call the functions on our instance of the contract so let hash = meme.get() and meme.set("QmPQZN62smuRxRACH8LGYHrNXQAKPbCEj8ciyagzYXmghR")
module.exports = function(deployer) {
  deployer.deploy(Meme);
};
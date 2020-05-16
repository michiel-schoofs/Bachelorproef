//Creating an artifact creates an ABI.
const Token = artifacts.require("./Token.sol");

//Deploy the token contract to the blockchain configured in truffle-config
//We run migrations with truffle migrate
//In our truffle console we can try to interact with the contract
//We get a reference by using Token.deployed().then(function(i){token=i;})
//We get the supply by using token.totalSupply().then(function(s){supply=s;})
//supply.toNumber();
//Every write function cost gas because of the nature of transactions
module.exports= function(deployer) {
  deployer.deploy(Token,1000000);
};

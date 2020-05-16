var Token = artifacts.require('./Token.sol');

//We run test with truffle test command
contract('Token', function(accounts){
    it('sets the total supply of the token',function(){
        return Token.deployed().then(function(instance){
            const contract = instance;
            return contract.totalSupply().then(function(s){
                assert.equal(s.toNumber(),1000000,'Sets the total supply to 1000000')
                return contract.balanceOf(accounts[0]).then(function(b){
                    assert.equal(b.toNumber(),1000000,'checks if tokens are allocated to initial address')
                })
            });
        })
    })
})
var Token = artifacts.require('./Token.sol');

//We run test with truffle test command
contract('Token', function(accounts){
    it('initializes the contract with the correct values',function(){
        return Token.deployed().then(function(instance){
            const contract = instance;
            return contract.name().then(function(name){
                assert.equal(name,"Yellow Existential Crisis coin",'token has the correct name');
                return contract.symbol().then(function(symb){
                    assert.equal(symb,'YEC','token has the correct symbol');
                    return contract.standard().then(function(stan){
                        assert.equal(stan,'v1.0','The current version of the token is initialized succefully');
                    })
                })
            })
        });
    })

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
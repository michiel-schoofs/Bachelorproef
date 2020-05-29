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

    it('transfers token ownership', function() {
        return Token.deployed().then(function(instance) {
          tokenInstance = instance;
          // Test `require` statement first by transferring something larger than the sender's balance
          return tokenInstance.transfer.call(accounts[1], 99999999999999999999999);
        }).then(assert.fail).catch(function(error) {
          return tokenInstance.transfer.call(accounts[1], 250000, { from: accounts[0] });
       /* }).then(function(success) {
          assert.equal(success, true, 'it returns true');
          return tokenInstance.transfer(accounts[1], 250000, { from: accounts[0] });
       */}).then(function(receipt) {
         /* assert.equal(receipt.logs.length, 1, 'triggers one event');
          assert.equal(receipt.logs[0].event, 'Transfer', 'should be the "Transfer" event');
          assert.equal(receipt.logs[0].args._from, accounts[0], 'logs the account the tokens are transferred from');
          assert.equal(receipt.logs[0].args._to, accounts[1], 'logs the account the tokens are transferred to');
          assert.equal(receipt.logs[0].args._value, 250000, 'logs the transfer amount');*/
          return tokenInstance.balanceOf(accounts[1]);
        }).then(function(balance) {
          assert.equal(balance.toNumber(), 250000, 'adds the amount to the receiving account');
          return tokenInstance.balanceOf(accounts[0]);
        }).then(function(balance) {
          assert.equal(balance.toNumber(), 750000, 'deducts the amount from the sending account');
        });
    });
})
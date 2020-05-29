const SimpleContract = artifacts.require('SimpleContract');

contract('SimpleContract', () => {
    it('Deployed Correctly',async() => {
        var contract = await SimpleContract.deployed();
        assert(contract.address !== "");
    });
});
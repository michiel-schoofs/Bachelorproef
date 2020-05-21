const SimpleContract = artifacts.require('HelloWorld');

contract('HelloWorld', () => {
    let contract;

    beforeEach(async ()=>{
        contract = await SimpleContract.deployed();
    })

    it('Deployed Correctly',async() => {
        assert(contract.address !== "");
    });

    it('Should return hello world', async()=>{
        const result = await contract.SayHello();
        assert(result === 'Hello World');
    });
});
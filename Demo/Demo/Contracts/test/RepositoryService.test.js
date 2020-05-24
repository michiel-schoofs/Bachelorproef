const RepositoryService = artifacts.require('RepositoryService');
const Repository = artifacts.require('Repository');
const UserService = artifacts.require('UserService');
const truffleAssert = require('truffle-assertions');

contract('RepositoryService',(accounts)=>{
    var contract;
    const account = accounts[0];
    const repoName = "My cool repo";

    beforeEach(async () => {
        const ad = await UserService.deployed();
        contract = await RepositoryService.new(ad.address);
    });

    it('contract is deployed', async ()=>{
        assert(contract.address != '');
    })

    it('Contract is initiliased with 1 as the current id', async()=>{
        const id = await contract.currentId.call({from:account});
        assert.strictEqual(id.toNumber(),1);
    })

    it('Contract has no names when initialised', async()=>{
        const ar = await contract.GetNames();
        assert.lengthOf(ar,0);
    })

    it('Repository creation with empty name should throw exception', async()=>{
        await truffleAssert.reverts(
            contract.CreateRepository("",{from: account})
        );
    });
    
    it('Repository Creation with duplicate name should throw exception', async()=>{
        await truffleAssert.passes(contract.CreateRepository(repoName,{from: account}))
        await truffleAssert.reverts(
            contract.CreateRepository(repoName,{from: account})
        );
        await truffleAssert.reverts(
            contract.CreateRepository(repoName,{from: accounts[1]})
        );
    })

    it('Repository creation should increment current id with one', async() => {
        const id = await contract.currentId.call({from:account});
        await truffleAssert.passes(contract.CreateRepository(repoName,{from: account}))
        const id2 = await contract.currentId.call({from:account});
        assert.strictEqual(id.toNumber()+1,id2.toNumber());
    })

    it('Repository creation should emit an event', async()=>{
        var result = await contract.CreateRepository(repoName,{from: account});
        truffleAssert.eventEmitted(result,"RepositoryAdded");
    })

    it('Repository not created should set the existsName function to false', async()=>{
        const exist = await contract.methods['CheckIfRepoExists(string)'](repoName);
        assert.isFalse(exist);
    })

    it('Repository created should set the existsName function to true', async() => {
        await truffleAssert.passes(contract.CreateRepository(repoName,{from: account}));
        const exist = await contract.methods['CheckIfRepoExists(string)'](repoName);
        assert.isTrue(exist);
    })

    it('Repository created names should appear in getNames function',async()=>{
        const ar1 = await contract.GetNames();
        await truffleAssert.passes(contract.CreateRepository(repoName,{from: account}));
        const ar2 = await contract.GetNames();       
        assert.strictEqual((ar1.length+1),ar2.length)
        assert.includeMembers(ar2,[repoName]);
    })

    it('Get Repository if not exist should throw exception', async()=>{
        await truffleAssert.reverts(
            contract.methods['getRepository(string)'](repoName,{from: account})
        );
    })

    it('Get Repository if it exists should give a non zero address', async()=>{
        await truffleAssert.passes(contract.CreateRepository(repoName,{from: account}));
        const address = await contract.methods['getRepository(string)'](repoName,{from: account});
        assert.notStrictEqual(address,0);
    })

    it('Name and ID of deployed repository are succesfully set', async()=>{
        const id = await contract.currentId.call();
        
        const result = await contract.CreateRepository(repoName,{from: account});
        const address = await contract.methods['getRepository(string)'](repoName,{from: account});
        const repository = await Repository.at(address);

        const name = await repository.name.call();
        const rid = await repository.id.call();

        assert.strictEqual(name, repoName);
        assert.strictEqual(id.toNumber(),rid.toNumber());
    })

    it('Owner set succesfully upon repository creation', async()=>{
        const result = await contract.CreateRepository(repoName,{from: account});
        const address = await contract.methods['getRepository(string)'](repoName,{from: account});
        const repository = await Repository.at(address);

        const owner = await repository.owner();
        assert.strictEqual(owner, account);
    })
})
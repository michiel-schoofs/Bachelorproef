//Using mocha testing framework and chai.js
require('chai').use(require('chai-as-promised')).should();

//Get the smart contract reference
const Meme = artifacts.require("Meme");

//What should happen to the contracts and have a reference to all accounts on our local instance of the blockchain
contract('Meme',(accounts)=>{
    let meme;

    //Before so for every describe
    before(async ()=>{
        meme = await Meme.deployed();
    })

    //Organise test examples
    describe('deployement', async ()=>{

        //Indiviual tests
        it('deploys succesfully',async () => {
            const ad = meme.address;
            assert.notEqual(ad,'');
            assert.notEqual(ad, 0x0);
            assert.notEqual(ad, null);
            assert.notEqual(ad, undefined);
        });
    })

    describe('storage', async()=>{
        it('updates the meme hash',async () =>{
            const hash = 'abc123';
            await meme.set(hash);
            const result = await meme.get();
            assert.strictEqual(hash,result);
        })
    })
})
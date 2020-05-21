const UserService = artifacts.require('UserService');
const truffleAssert = require('truffle-assertions');

contract('UserService',(accounts)=>{
    var contract;
    const account = accounts[0];
    const username = "Bee Benson"

    beforeEach(async () => {
        contract = await UserService.new();
    });

    it('Contract is deployed',()=>{
        assert(contract.address != '');
    })

    it('If the user has no account the userHasAccount function should return false',async ()=>{
        var result = await contract.userHasAccount({ from: account });
        assert.isFalse(result);
    })

    it('Non existing username should make the function usernameExists return false', async ()=>{
        var result = await contract.usernameExists(username);
        assert.isFalse(result);
    });

    it('Adding a user fires user added event with the provided username', async () => {
        var result = await contract.addUser(username);
        await truffleAssert.eventEmitted(result, 'NewUserAdded',(e)=>{
            return e._username === username;
        });
    });

    it('If an account already has a username should throw an exception', async () => {
        var username2 = "Buzz Stinger";
        var message = "This user already has an account";

        await truffleAssert.passes(contract.addUser(username, {from: account}));
        await truffleAssert.reverts(
            contract.addUser(username2, {from: account}),
            null,
            message
        );
    });

    it('Two accounts registering the same username should throw exception', async () => {
        var account2 = accounts[1];

        await truffleAssert.passes(contract.addUser(username, {from: account}));
        await truffleAssert.reverts(
            contract.addUser(username, {from: account2})
        );
    });

    it('If the user has an account the userHasAccount function should return true',async ()=>{
        await truffleAssert.passes(contract.addUser(username, {from: account}));

        var result = await contract.userHasAccount({ from: account });
        assert.isTrue(result);
    })

    it('Get user function when username does not exist should throw exception', async() => {
        await truffleAssert.reverts(
            contract.getUser(username,{from:account})
        )
    })

    it('Get user function when username exist should return address', async() => {
        await truffleAssert.passes(contract.addUser(username, {from:account}));
        var result = await contract.getUser(username,{from:account});
        assert.strictEqual(result, account);
    })

    it('Get Amount of users should increment after adding a user', async() =>{
        var initial = await contract.getAmountOfUsers();
        await truffleAssert.passes(contract.addUser(username, {from:account}));
        var result = await contract.getAmountOfUsers();
        assert.strictEqual((initial.toNumber()+1), result.toNumber());
    });

    it('Remove user with non existing user should throw exception', async() => {
        await truffleAssert.reverts(
            contract.removeUser(username)
        );
    });

    it('Remove existing user should fire event with username', async() => {
        await truffleAssert.passes(contract.addUser(username, {from:account}));
        var result = await contract.removeUser(username);
        truffleAssert.eventEmitted(result, 'UserDeleted',(e)=>{
            return e._username === username;
        });
    });

    it('Remove existing user, removes username existing', async() => {
        await truffleAssert.passes(contract.addUser(username, {from:account}));
        var resultBeforeDelete = await contract.usernameExists(username);

        await truffleAssert.passes( await contract.removeUser(username));
        var resultAfterDelete = await contract.usernameExists(username);

        assert.isTrue(resultBeforeDelete);
        assert.isFalse(resultAfterDelete);
    });

    it('Remove someone else should throw an exception', async() => {
        var account2 = accounts[1];
        await truffleAssert.passes(contract.addUser(username, {from:account}));
        await truffleAssert.reverts(
            contract.removeUser(username, {from:account2})
        );
    })
});
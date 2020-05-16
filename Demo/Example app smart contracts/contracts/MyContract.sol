//specify the version of solidity to use
pragma solidity >=0.4.24;

contract Concepts {
    //A getter is automatically created with a public state variable
    string public name;
    //The way we define a constant value, can not be changed
    bool public constant alwaysTrue=true;
    //Only make this visible to our contract, people can't view the owner but we can reference it in our contract
    address internal owner;

    //Epoch time
    uint internal openingtime;

    //Modifiers are a way of limiting when a function can execute
    modifier onlyOwner(){
        //If the expression evaluates to false then throw exception
        //msg is metadata that gets added to every call, so we verify that the person calling this function is the owner
        require(msg.sender == owner,"Only the owner of this contract can run this command");
        //If normal execution don't do anything
        _;
    }

    //Allow the owner to change control
    function changeOwner(address _newOwner) public onlyOwner {
        owner = _newOwner;
    }

    //Make a way that our owner can change the opening time
    function setOpeningtime(uint _openingTime) public onlyOwner {
        openingtime = _openingTime;
    }

    //The view modifier makes it clear we only intend this function to read data and not actually modify the state variables
    function getOpeningtime() public view returns (uint) {
        return openingtime;
    }

    //Enumerations also exist
    enum ContractStatus{
        Open, Closed
    }

    //We can use enumerations in much the same way
    function getStatus() public view returns (ContractStatus){
        if(block.timestamp >= openingtime) {
            return ContractStatus.Open;
        }

        return ContractStatus.Closed;
    }

    // A way to represent custom datastructors
    struct People{
        string _firstname;
        string _lastname;
        uint8 _age;
    }

    //Comparable to a dictonary so a key value store
    mapping(uint8 => People) PeopleAr;
    //There's no way to querry a mapping for the keys or count so we have to manually add those variables
    uint8[] public ids = new uint8[](0);
    uint8 public size = 0;

    //This is how we set a value in our mapping and also make an instance of a struct
    function addPeople(string memory _fn, string memory _ln, uint8 _age) public returns (uint8) {
        //There's no lists but the concept of dynamic arrays function in much the same way
        ids.push(size);
        //Set a new person object with the id == size and instantiate a new Person object
        PeopleAr[size] = People(_fn,_ln,_age);
        //Short hand operator of size = size+1
        size++;
    }

    //Gets called when the contract is first deployed
    constructor(string memory _name) public {
        //set state variable
        name = _name;
        owner = msg.sender;
    }

}
import React, { Component } from 'react';
import './App.css';
import ipfsClient from 'ipfs-http-client';
import Web3 from 'web3';
import Meme from '../abis/Meme.json';

//Connect to a public node we use infura
const ipfs = ipfsClient({
  host: 'ipfs.infura.io',port:5001, protocol:'https'
})

class App extends Component {
  constructor(props){
    super(props);
    this.state = {
      buffer:null,
      memehash:'QmPQZN62smuRxRACH8LGYHrNXQAKPbCEj8ciyagzYXmghR',
      account:null,
      contract: null
    };  
  }

  captureFile = (event) => {
    //Make sure that default behaviour is not executed.
    event.preventDefault();
    //Capture the file
    const file = event.target.files[0];
    //Way to convert file to a buffer
    const reader = new window.FileReader();
    reader.readAsArrayBuffer(file);
    reader.onloadend = () => {
      //Converted to buffer of uint8[]
      const buf = Buffer(reader.result);
      //Save it in the state. That way we can use it in other functions
      this.setState({buffer: buf});
    }
  }

  //Get the account
  //Get the network
  //Get the smartcontract
  //Get the memehash
  async loadBlockchainData(){
    const web3 = window.web3;
    //Get all the accounts in the wallet -> metamask so only one
    web3.eth.getAccounts().then((ac)=>{
      this.setState({account:ac[0]});
    });
    
    //We need the abi and the network id to get a reference to our smartcontract
    const networkid = await web3.eth.net.getId();
    //We get the information about the network, contains a reference to the address of the smart contract.
    const networkData = Meme.networks[networkid];
    if(networkData){
      const abi = Meme.abi;
      const address = networkData.address;
      console.log(address);
      //Gets the actual reference to the contract on the network
      const contract = web3.eth.Contract(abi,address);
      this.setState({contract});
      const memehash = await contract.methods.get().call();
      this.setState({memehash})
    }else{
      //There was no deployment on this specific network
      window.alert("Smart contract not deployed to network")
    }
  }

  //Binds on mount of component simulair to the way Vue mount works
  async componentWillMount(){
    await this.loadWeb3();
    await this.loadBlockchainData();
  }


  //Loads in our Web3 provider using metamask
  async loadWeb3(){
    if(window.ethereum){
      window.web3 = new Web3(window.ethereum);
      await window.ethereum.enable();
    }if(window.web3){
      //Wired up local network to web
      window.web3 = new Web3(new Web3.providers.HttpProvider('http://127.0.0.1:7545'));
      await window.ethereum.enable();
    }else{
      //If metamask is not installed
      window.alert("Please use metamask");
    }
  }

  //example hash: "QmPQZN62smuRxRACH8LGYHrNXQAKPbCEj8ciyagzYXmghR"
  //example url: "https://ipfs.infura.io/ipfs/QmPQZN62smuRxRACH8LGYHrNXQAKPbCEj8ciyagzYXmghR"
  onSubmit = (event) => {
    event.preventDefault();
    console.log("setting to ipfs")
    //Add data to node, second argument is a callback function
    ipfs.add(this.state.buffer,(error,result)=>{
      console.log('IPFS result',result);

      if(error){
        console.log('error',error)
        return;
      }
      
      this.state.contract.methods.set(result[0].hash).send({
        from: this.state.account
      }).then((r)=>{
        this.setState({memehash: result[0].hash});
      });
    })
    //Step 2 is store file on the blockchain

  }

  render() {
    return (
      <div>
        <nav className="navbar navbar-dark fixed-top bg-dark flex-md-nowrap p-0 shadow">
          <a
            className="navbar-brand col-sm-3 col-md-2 mr-0"
            href="http://www.dappuniversity.com/bootcamp"
            target="_blank"
            rel="noopener noreferrer"
          >
            Meme of the day
          </a>
          <ul className="navbar-nav px-3">
            <li className="nav-item text-nowrap d-none d-sm-none d-sm-block">
              <small className="text-white">{this.state.account}</small>
            </li>
          </ul>
        </nav>
        <div className="container-fluid mt-5">
          <div className="row">
            <main role="main" className="col-lg-12 d-flex text-center">
              <div className="content mr-auto ml-auto">
                <a
                  href="http://www.dappuniversity.com/bootcamp"
                  target="_blank"
                  rel="noopener noreferrer"
                >
                  <img src={`https://ipfs.infura.io/ipfs/${this.state.memehash}`} className="App-logo" alt="img"/>
                </a>
                <p>&nbsp;</p>
                <h2>Change meme</h2>
                <form onSubmit={this.onSubmit}>
                  <input type="file" onChange={this.captureFile} />
                  <input type="submit"/>
                </form>
              </div>
            </main>
          </div>
        </div>
      </div>
    );
  }
}

export default App;

import React, { Component } from 'react';
import './App.css';
import ipfsClient from 'ipfs-http-client';

//Connect to a public node we use infura
const ipfs = ipfsClient({
  host: 'ipfs.infura.io',port:5001, protocol:'https'
})

class App extends Component {
  constructor(props){
    super(props);
    this.state = {
      buffer:null,
      memehash:'QmPQZN62smuRxRACH8LGYHrNXQAKPbCEj8ciyagzYXmghR'
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

  //example hash: "QmPQZN62smuRxRACH8LGYHrNXQAKPbCEj8ciyagzYXmghR"
  //example url: "https://ipfs.infura.io/ipfs/QmPQZN62smuRxRACH8LGYHrNXQAKPbCEj8ciyagzYXmghR"
  onSubmit = (event) => {
    event.preventDefault();
    console.log("setting to ipfs")
    //Add data to node, second argument is a callback function
    ipfs.add(this.state.buffer,(error,result)=>{
      this.setState({memehash: result[0].hash});
      console.log('IPFS result',result);
      if(error){
        console.log('error',error)
        return;
      }
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

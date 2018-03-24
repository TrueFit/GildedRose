import React from 'react';
import * as ApiCall from '../../apiCalls';

export default class EndOfDayPage extends React.Component{
    constructor(){
        super();
        state:{
            saving: false
        }

        this.preformProcess = this.preformProcess.bind(this);
        this.redirectToTrash = this.redirectToTrash.bind(this);
        this.handleError = this.handleError.bind(this);
    }

    componentWillMount(){
        this.setState({saving: false});
    }

    preformProcess(){
        this.setState({saving:true});
        let postUrl = "http://localhost:5000/api/Inventory/end-of-day";
        ApiCall.PostToInventoryApi(postUrl, this.redirectToTrash, this.handleError);
        // call Post to end of day
        //$.post(postUrl).done(this.redirectToTrash);
        /*
        fetch(postUrl,{
            method: "POST"           
        }).then(response => this.redirectToTrash())
        .catch(error => console.log(error));
        */
    }

    handleError(error){
        console.log(error);
    }

    redirectToTrash(){
        window.location = "http://localhost:5000/trash";
        this.setState({saving:false});
    }


    render(){
        return(
            <div>
                <h2>End Of Day Processing</h2>
                <p>This will preform the End of Day Process.</p>
                <p>Once the End Of Day process completes, you will be redirected to the Trash Page</p>
                <p>Click to Proceed.</p>
                <input type="submit" 
                    value={this.state.saving ? "Processing..." : "Process End Of Day"}
                    disabled={this.state.saving}
                    className="btn btn-primary"
                    onClick={this.preformProcess}
                />
            </div>
        );

    }
}
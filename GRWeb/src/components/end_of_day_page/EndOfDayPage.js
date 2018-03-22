import React from 'react';

export default class EndOfDayPage extends React.Component{
    constructor(){
        super();

        this.preformProcess = this.preformProcess.bind(this);
    }

    preformProcess(){
        let postUrl = "http://localhost:5000/api/Inventory/end-of-day";
        // call Post to end of day
        //$.post(postUrl).done(this.redirectToTrash);
        fetch(postUrl,{
            method: "POST"           
        }).then(response => this.redirectToTrash())
        .catch(error => console.log(error));
    }

    redirectToTrash(){
        window.location = "http://localhost:5000/trash";
    }


    render(){
        return(
            <div>
                <h2>End Of Day Processing</h2>
                <p>This will preform the End of Day Process.</p>
                <p>Click to Proceed.</p>
                <input type="submit" 
                    value="Process End Of Day"
                    className="btn btn-primary"
                    onClick={this.preformProcess}
                />
            </div>
        );

    }
}
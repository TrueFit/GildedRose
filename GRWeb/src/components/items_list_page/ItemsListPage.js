import React from 'react';
import ItemList from '../common/ItemList';
import ShowTrashCheckBox from '../common/ShowTrashCheckBox';
import * as ApiCall from '../../apiCalls';

export default class ItemsListPage extends React.Component {
    constructor() {
        super();
        this.state = { 
            loading: true,
            showTrash: false,
            messsage: "",
            listOfItems: [
            ]
        };

        this.handleError = this.handleError.bind(this);
        this.handleResponse = this.handleResponse.bind(this);
        this.setShowTrashState = this.setShowTrashState.bind(this);
    }

    // API Call
    getInventory(){
        let apiUrl= "http://localhost:5000/api/Inventory";
        ApiCall.CallInventoryApi(apiUrl, this.handleResponse, this.handleError);
        /*
        fetch(apiUrl)
        .then(respone => respone.json())
        .then(items => this.handleResponse(items))
        .catch(error => handleError(error));
        */
    }

   handleResponse(items){
        if (items.length >0){
            this.setState({listOfItems: items});
        }
        else{
            console.log("No Items Returned");
        }
    }

    handleError(error){
        console.log(error);
    }

    setShowTrashState(){
        this.setState({showTrash: !this.state.showTrash});
    }

    componentDidMount(){
        this.getInventory();
    }

    render(){
        const items = this.state.listOfItems;
        return(
            <div>
                <h2>Inventory Items</h2>
                <ShowTrashCheckBox onClick={this.setShowTrashState}/>
                <ItemList items={items} showTrash={this.state.showTrash}/>
            </div>
        );
    }
}
import React from 'react';
import ItemList from '../common/ItemList';
import {ApiCall} from '../../apiCalls';

export default class ItemsListPage extends React.Component {
    constructor() {
        super();
        this.state = { 
            loading: true,
            messsage: "",
            listOfItems: [
            ]
        };

        this.handleError = this.handleError.bind(this);
        this.handleResponse = this.handleResponse.bind(this);
    }

    // API Call
    getInventory(){
        let apiUrl= "http://localhost:5000/api/Inventory";
        ApiCall(apiUrl, this.handleResponse, this.handleError);
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

    componentDidMount(){
        this.getInventory();
    }

    render(){
        const items = this.state.listOfItems;
        return(
            <div>
            <h2>Inventory Items</h2>
                <ItemList items={items} />
            </div>
        );
    }
}
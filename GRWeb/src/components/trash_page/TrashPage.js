import React from 'react';
import ItemList from '../common/ItemList';
import {ApiCall} from '../../apiCalls';


export default class TrashPage extends React.Component{
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
    getTrash(){
        let apiUrl= "http://localhost:5000/api/Inventory/trash";
        ApiCall(apiUrl, this.handleResponse, this.handleError);
        /*
        fetch("http://localhost:5000/api/Inventory/trash")
        .then(respone => respone.json())
        .then(items => this.handleResponse(items))
        .catch(error => console.error(error));
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
        this.getTrash();
    }

    render(){
        const items = this.state.listOfItems;
        return(
            <div>
            <h2>Items for Trash</h2>
                <ItemList items={items} />
            </div>
        );
    }
}
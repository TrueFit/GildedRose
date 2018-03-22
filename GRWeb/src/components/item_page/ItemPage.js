import React from 'react';
import SelectInput from '../common/SelectInput';
import DisplayField from '../common/DisplayField';

import {ApiCall} from '../../apiCalls';

const ItemsPage = ({ match: { params: {itemName}} }) =>{
    return <ItemPageComponent itemName={itemName} />
}

const emptyItem = {name: "", category: "", sellIn: "", quality: ""};

class ItemPageComponent extends React.Component {
    constructor(){
        super();
        this.state = {
            loading: false,
            currentItem: "",
            item: emptyItem,
            listOfItems:[]
        };

        this.getInventory = this.getInventory.bind(this);
        this.getItem = this.getItem.bind(this);
        this.handleItemResponse = this.handleItemResponse.bind(this);
        this.handleResponse = this.handleResponse.bind(this);
        this.handleError = this.handleError.bind(this);
        this.onSelectChange = this.onSelectChange.bind(this);
    }

    onSelectChange(event){
        let field = event.target.name;
        let itemName = event.target.value;
        let newUrl = "http://localhost:5000/Item/"+itemName;
        window.location =newUrl;
    }

    // API Calls
    getItem(){
        let apiUrl = "http://localhost:5000/api/Inventory/item/"+ this.state.currentItem;
        ApiCall(apiUrl, this.handleItemResponse, this.handleError);
    }

    getInventory(){
        let apiUrl = "http://localhost:5000/api/Inventory";
        ApiCall(apiUrl, this.handleResponse, this.handleError);
    }


    // Response
    handleItemResponse(item){
        if (item != null){
            this.setState({item: item});
        }
        else{
            this.setState({item: emptyItem});
        }
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

    componentWillMount(){
        if (this.props.itemName != null){
            this.setState({currentItem: this.props.itemName});
        }
    }

    componentDidMount(){
        if (this.state.currentItem != ""){
            this.getItem();
        }
        this.getInventory();
    }

    render(){
        return(
            <div>
                <SelectInput 
                    name="name"
                    label="Item"
                    value={this.state.item.name}
                    defaultOption="Select Item"
                    onChange={this.onSelectChange}
                    options={itemsFormattedForDropDown(this.state.listOfItems)}
                />
                <DisplayField
                    name="category"
                    label="Category"
                    value={this.state.item.category} 
                />
                <DisplayField 
                    name="quality"
                    label="Quality"
                    value={`${this.state.item.quality}`}
                />
                <DisplayField
                    name="sellIn"
                    label="Sell In"
                    value={`${this.state.item.sellIn}`}
                />
            </div>
        );
    }
}

function itemsFormattedForDropDown(items) {
    return items.map (item => {
        return{
            value: item.name,
            text: item.name
        };
    });
}

export default ItemsPage;
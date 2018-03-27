import React from 'react';
import {PropTypes} from 'prop-types';
import {bindActionCreators} from 'redux';
import {connect} from 'react-redux';

import SelectInput from '../common/SelectInput';
import DisplayField from '../common/DisplayField';
import * as itemActions from '../../actions/itemActions';
import * as itemsListActions from '../../actions/itemsListActions';

class ItemPageComponent extends React.Component {
    constructor(props, context){
        super(props, context);
        this.state ={
            item: Object.assign({}, this.props.item)
        }

        this.onSelectChange = this.onSelectChange.bind(this);
    }

    onSelectChange(event){       
        let field = event.target.name;
        let itemName = event.target.value;

        let newUrl = "http://localhost:5000/Item/"+itemName;
        window.location = newUrl;
    }

    componentDidMount(){
        this.loadItemInformation(this.props.itemName);
    }

    componentWillReceiveProps(nextProps){
        this.setState({item: nextProps.item});
    }

    loadItemInformation(itemName){
        this.props.itemActions.loadItem(itemName);
    }

    render(){
        const listOfItems = this.props.itemsList;
        const item = this.state.item;
        return(
            <div>
                <SelectInput 
                    name="name"
                    label="Item"
                    value={item.name}
                    defaultOption="Select Item"
                    onChange={this.onSelectChange}
                    options={itemsFormattedForDropDown(listOfItems)}
                />
                <DisplayField
                    name="category"
                    label="Category"
                    value={item.category} 
                />
                <DisplayField 
                    name="quality"
                    label="Quality"
                    value={`${item.quality}`}
                />
                <DisplayField
                    name="sellIn"
                    label="Sell In"
                    value={`${item.sellIn}`}
                />
            </div>
        );
    }
}

ItemPageComponent.propTypes ={
    item: PropTypes.object.isRequired,
    itemsList: PropTypes.array.isRequired
}

// set the props from the store state that is passed into the form
function mapStateToProps(state, ownProps){
    return{
        item: state.item,
        itemsList: state.itemsList
    };
}

// connect the form to a dispatch
function mapDisptachToProps(dispatch){
    return{
        itemActions: bindActionCreators(itemActions, dispatch)
    };
}

function itemsFormattedForDropDown(items) {
    return items.map (item => {
        return{
            value: item.name,
            text: item.name
        };
    });
}

export default connect(mapStateToProps, mapDisptachToProps)(ItemPageComponent)
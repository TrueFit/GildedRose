import React from 'react';
import {PropTypes} from 'prop-types';
import {bindActionCreators} from 'redux';
import {connect} from 'react-redux';

import ItemList from '../common/ItemList';
import ShowTrashCheckBox from '../common/ShowTrashCheckBox';
import * as itemsListActions from '../../actions/itemsListActions';

class ItemsListPage extends React.Component {
    constructor(props, context) {
        super(props, context);
        this.state ={ // setup mutable state to allow for show trash checkboc click
            itemsList: Object.assign([], this.props.itemsList),
            showTrash: false
        }

        this.setShowTrashState = this.setShowTrashState.bind(this);
    }
    
    setShowTrashState(){
        this.setState({showTrash: !this.state.showTrash});
    }

    componentDidMount(){
        this.props.actions.loadAllItems();
    }

    componentWillReceiveProps(nextProps){
        // Thill will update the form when the api call returns
        this.setState({itemsList: Object.assign([], nextProps.itemsList)});
    }

    render(){
        // get the items list from the local state
        const items = this.state.itemsList;
        return(
            <div>
                <h2>Inventory Items</h2>
                <ShowTrashCheckBox onClick={this.setShowTrashState}/>
                <ItemList items={items} showTrash={this.state.showTrash}/>
            </div>
        );
    }
}

ItemsListPage.propTypes ={
    itemsList: PropTypes.array.isRequired
}

// set the props from the store state that is passed into the form
function mapStateToProps(state, ownProps){
    return{
        itemsList: state.itemsList
    };
}

// connect the form to a dispatch
function mapDisptachToProps(dispatch){
    return{
        actions: bindActionCreators(itemsListActions, dispatch)
    };
}

export default connect(mapStateToProps, mapDisptachToProps)(ItemsListPage)





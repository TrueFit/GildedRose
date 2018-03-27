import React from 'react';
import {PropTypes} from 'prop-types';
import {bindActionCreators} from 'redux';
import {connect} from 'react-redux';

import ItemList from '../common/ItemList';
import * as trashActions from '../../actions/trashActions';

class TrashPage extends React.Component{
    constructor(props, context) {
        super(props, context);
        this.state = { 
            loading: true,
            trashList: Object.assign([], this.props.trashList)
        };
    }

    componentDidMount(){
        this.props.actions.loadTrashItems();
    }

    componentWillReceiveProps(props, ownProps){
        this.setState({trashList: Object.assign([], props.trashList)});
    }

    render(){
        const items = this.state.trashList;
        return(
            <div>
            <h2>Items for Trash</h2>
                <ItemList items={items} showTrash={true} />
            </div>
        );
    }
}

TrashPage.propTypes = {
    trashList: PropTypes.array.isRequired
}

function mapStateToProps(state, ownProps){
    return{
        trashList: state.trashList
    };
}

function mapDispatchToProps(dispatch){
    return{
        actions: bindActionCreators(trashActions, dispatch)
    };
}

export default connect(mapStateToProps, mapDispatchToProps)(TrashPage);

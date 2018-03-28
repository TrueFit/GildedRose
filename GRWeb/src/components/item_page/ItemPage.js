import React from 'react';
import {PropTypes} from 'prop-types';
import {bindActionCreators} from 'redux';
import {connect} from 'react-redux';

import SelectInput from '../common/SelectInput';
import DisplayField from '../common/DisplayField';
import * as itemActions from '../../actions/itemActions';
import * as itemsListActions from '../../actions/itemsListActions';
import ItemPageComponent from './ItemPageComponent';


const ItemPage = ({ match: { params: {itemName}} }) =>{
    return <ItemPageComponent itemName={itemName}/>
}

export default connect()(ItemPage)
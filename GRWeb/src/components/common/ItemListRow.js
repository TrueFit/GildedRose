import React from 'react';
import {PropTypes} from "prop-types";
import {Link} from 'react-router-dom';

const ItemListRow = ({item, showTrash}) => {
    if (!showTrash && item.quality == 0){
        return null;
    }
    else{
        return(
            <tr>
                <td><Link to={"/item/" + item.name}>{item.name}</Link></td>
                <td>{item.category}</td>
                <td>{item.quality}</td>
                <td>{item.sellIn}</td>
            </tr>
        );
    }
};


ItemListRow.propTypes ={
    item: PropTypes.object.isRequired,
    showTrash: PropTypes.bool
};

export default ItemListRow
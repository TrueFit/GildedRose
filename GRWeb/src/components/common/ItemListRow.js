import React from 'react';
import {PropTypes} from "prop-types";

const ItemListRow = ({item, showTrash}) => {
    if (!showTrash && item.quality == 0){
        return null;
    }
    else{
        return(
            <tr>
                <td><a href={"/item/" + item.name}>{item.name}</a></td>
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
import React, {PropTypes} from 'react';
import {Link} from 'react-router-dom';

const ItemListRow = ({item}) => {
    return(
        <tr>
            <td><Link to={"/item/" + item.name}>{item.name}</Link></td>
            <td>{item.category}</td>
            <td>{item.quality}</td>
            <td>{item.sellIn}</td>
        </tr>
    );
};

ItemListRow.propTypes ={
    item: PropTypes.object.isRequired
};

export default ItemListRow
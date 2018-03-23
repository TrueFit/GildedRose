import React from 'react';
import {PropTypes} from "prop-types";
import ItemListRow from './ItemListRow';

const ItemList = ({items, showTrash}) => {
    return(
        <table className="table">
            <thead>
                <tr>
                    <th>Name</th>
                    <th>Category</th>
                    <th>Quality</th>
                    <th>Sell In</th>
                </tr>
            </thead>
            <tbody>
                {items.map(item => 
                    <ItemListRow key={item.name} item={item} showTrash={showTrash} />
                )}
            </tbody>
        </table>
    );
};

ItemList.propTypes ={
    items: PropTypes.array.isRequired,
    showTrash: PropTypes.bool
};

ItemList.defaultProps = {
    showTrash: false
}

export default ItemList;
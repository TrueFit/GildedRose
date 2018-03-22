import React, {PropTypes} from 'react';
import ItemListRow from './ItemListRow';

const ItemList = ({items}) => {
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
                    <ItemListRow key={item.name} item={item} />
                )}
            </tbody>
        </table>
    );
};

ItemList.propTypes ={
    items: PropTypes.array.isRequired
};

export default ItemList;
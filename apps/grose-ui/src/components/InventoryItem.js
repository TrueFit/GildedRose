import React, { useState, useEffect } from 'react';

const InventoryItem = props => {
    const [item, updateItem] = useState(props);

    useEffect(() => {
        updateItem(props);
    }, [props]);

    return (
        <div className="InventoryItem">
            <div>{item.name}</div>
            <div>{item.category}</div>
            <div>{item.sellIn}</div>
            <div>{item.quality}</div>
        </div>
    );
}

export default InventoryItem;
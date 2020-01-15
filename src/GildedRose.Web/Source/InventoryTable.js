import React from "react";
import PropTypes from "prop-types";
import { inventoryCollectionType } from "./commonPropTypes";
import InventoryRow from "./InventoryItemRow";

function InventoryTable({ items, removeItem }) {
    return (
        <table className="table">
            <thead>
                <tr>
                    <th>Item name</th>
                    <th>Category</th>
                    <th>Sell In</th>
                    <th>Quality</th>
                    <th />
                </tr>
            </thead>
            <tbody>
                {items.map(item => <InventoryRow key={item.id} item={item} removeItem={removeItem} />)}
            </tbody>
        </table>
    );
}

InventoryTable.propTypes = {
    items: inventoryCollectionType.isRequired,
    removeItem: PropTypes.func.isRequired
};

export default InventoryTable;
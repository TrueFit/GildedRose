import React from "react";
import PropTypes from "prop-types";
import { inventoryItemType } from "./commonPropTypes";

function InventoryRow({ item, removeItem }) {
    return (
        <tr>
            <td>{item.name}</td>
            <td>{item.category}</td>
            <td>{item.sellIn}</td>
            <td>{item.quality}</td>
            <td><button type="button" className="btn btn-sm btn-outline-primary" onClick={() => removeItem(item.id)}>Sell / Trash</button></td>
        </tr>
    );
}

InventoryRow.propTypes = {
    item: inventoryItemType.isRequired,
    removeItem: PropTypes.func.isRequired
};

export default InventoryRow;
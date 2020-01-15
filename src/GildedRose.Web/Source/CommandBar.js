import React from "react";
import PropTypes from "prop-types";

function CommandBar({ advanceToNextDay, filterByName, filterTrashOnly, itemNames, listAllInventory, selectedName }) {
    itemNames.sort((a, b) => a.toUpperCase().localeCompare(b.toUpperCase()));

    return (
        <div className="command-bar">
            <button className="btn btn-primary mr-2" type="button" onClick={listAllInventory}>List all inventory</button>
            <button className="btn btn-primary mr-2" type="button" onClick={filterTrashOnly}>List trash (Quality = 0)</button>
            <form className="form-inline">
                <label className="mr-2">Filter by item name:</label>
                <select className="form-control" onChange={filterByName} value={selectedName}>
                    <option value="">(No filter)</option>
                    {itemNames.map(c => <option key={c} value={c}>{c}</option>)}
                </select>
            </form>
            <button style={{ marginLeft: "auto" }} className="btn btn-warning" type="button" onClick={advanceToNextDay}>Next day &gt;</button>
        </div>
    );
}

CommandBar.propTypes = {
    advanceToNextDay: PropTypes.func.isRequired,
    filterByName: PropTypes.func.isRequired,
    filterTrashOnly: PropTypes.func.isRequired,
    itemNames: PropTypes.arrayOf(PropTypes.string.isRequired).isRequired,
    listAllInventory: PropTypes.func.isRequired,
    selectedName: PropTypes.string.isRequired
};

export default CommandBar;
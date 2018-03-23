import React from 'react';
import {PropTypes} from "prop-types";

const ShowTrashCheckBox = ({onClick})=>{
    return(
        <div>
            <input type = "checkbox" id="showTrashCheckBox" onClick={onClick}/>
            <label id="showTrashItemsLabel" htmlFor="showTrashCheckBox">Show Trash Items</label>
        </div>
    );
};

ShowTrashCheckBox.propTypes = {
    onClick: PropTypes.func.isRequired
};

export default ShowTrashCheckBox;
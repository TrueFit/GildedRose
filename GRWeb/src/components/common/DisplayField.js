import React from 'react';
import {PropTypes} from "prop-types";

const DisplayField = ({name, label, value}) => {
    return (
        <div className = "formGroup display-field" >
        <label htmlFor = {name} > {label}: </label> 
        <div className = "field" >
           {value}
        </div>  
        </div>
    )
};

DisplayField.propTypes = {
    name: PropTypes.string.isRequired,
    label: PropTypes.string.isRequired,
    value: PropTypes.string
};

export default DisplayField;
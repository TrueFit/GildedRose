import React, {PropTypes} from 'react';

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
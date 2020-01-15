import React from "react";
import PropTypes from "prop-types";

function AddForm({ category, handleInputChange, name, quality, sellIn, submit, suggestedCategories }) {

    const parsedQuality = Number.parseFloat(quality);
    const parsedSellIn = Number.parseFloat(sellIn);
    const canSubmit = category && name && Number.isInteger(parsedQuality) && parsedQuality >= 0 && Number.isInteger(parsedSellIn);

    suggestedCategories.sort((a,b) => a.toUpperCase().localeCompare(b.toUpperCase()));

    return (
        <form className="add-form" onSubmit={canSubmit ? submit : null}>
            <div className="form-row">
                <div className="form-group col-lg-3">
                    <label htmlFor="add_name">Name</label>
                    <input name="name" type="text" className="form-control" id="add_name" value={name} onChange={handleInputChange} />
                </div>
                <div className="form-group col-lg-3">
                    <label htmlFor="add_category">Category</label>
                    <input name="category" type="text" className="form-control" id="add_category" value={category} onChange={handleInputChange} list="category_list" />
                    <datalist id="category_list">
                        {suggestedCategories.map(c => <option key={c} value={c} />)}                        
                    </datalist>
                </div>
                <div className="form-group col-lg-2">
                    <label htmlFor="add_quality">Initial quality</label>
                    <input name="quality" type="number" className="form-control" id="add_quality" value={quality} onChange={handleInputChange} />
                </div>
                <div className="form-group col-lg-2">
                    <label htmlFor="add_sellIn">Initial sell in days</label>
                    <input name="sellIn" type="number" className="form-control" id="add_sellIn" value={sellIn} onChange={handleInputChange} />
                </div>
                <div className="form-group col-lg-2">
                    <button type="submit" className="btn btn-primary" disabled={!canSubmit}>Add new item</button>
                </div>
            </div>
        </form>
    );
}

AddForm.propTypes = {
    category: PropTypes.string.isRequired,
    handleInputChange: PropTypes.func.isRequired,
    name: PropTypes.string.isRequired,
    quality: PropTypes.string.isRequired,
    sellIn: PropTypes.string.isRequired,
    submit: PropTypes.func.isRequired,
    suggestedCategories: PropTypes.arrayOf(PropTypes.string).isRequired
};

export default AddForm;
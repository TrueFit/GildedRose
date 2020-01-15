import React from "react";
import PropTypes from "prop-types";
import AddForm from "./AddForm";

class AddFormContainer extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            name: "",
            category: "",
            quality: "0",
            sellIn: "0"
        };

        this.handleInputChange = this.handleInputChange.bind(this);
        this.submit = this.submit.bind(this);
    }

    handleInputChange(event) {
        this.setState({
            [event.target.name]: event.target.value
        });
    }

    submit(event) {
        event.preventDefault();
        this.props.addItem(this.state.name, this.state.category, parseInt(this.state.quality), parseInt(this.state.sellIn));
        this.setState({
            name: "",
            category: "",
            quality: "0",
            sellIn: "0"
        });
    }

    render() {
        const { name, category, quality, sellIn } = this.state;
        return <AddForm name={name} category={category} quality={quality} sellIn={sellIn} submit={this.submit} handleInputChange={this.handleInputChange} suggestedCategories={this.props.categories} />;
    }
}

AddFormContainer.propTypes = {
    addItem: PropTypes.func.isRequired,
    categories: PropTypes.arrayOf(PropTypes.string).isRequired
};

export default AddFormContainer;
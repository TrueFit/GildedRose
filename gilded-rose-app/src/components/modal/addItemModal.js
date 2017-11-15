import React from 'react';
import { branch } from 'baobab-react/higher-order';
import BaseComponent from '../../components/baseComponent';
import _ from 'lodash';

import { Grid, Row, Col, PageHeader, Button, Modal, FormControl, FormGroup, Form, ControlLabel } from 'react-bootstrap';

import FontAwesome from 'react-fontawesome';

import { hideModal, showModal, addItem } from '../../actions/appActions';

const ITEM_CATEGORY_SULFURAS = "Sulfuras";
const ITEM_CATEGORY_CONJURED = "Conjured";
const ITEM_CATEGORY_BACKSTAGE_PASSES = "Backstage passes";
const ITEM_CATEGORY_FOOD = "Food";
const ITEM_CATEGORY_WEAPON = "Weapon";
const ITEM_CATEGORY_ARMOR = "Armor";
const ITEM_CATEGORY_POTION = "Potion";
const ITEM_CATEGORY_MISC = "Misc";

const ITEM_CATEGORIES = [
    ITEM_CATEGORY_CONJURED,
    ITEM_CATEGORY_SULFURAS,
    ITEM_CATEGORY_BACKSTAGE_PASSES,
    ITEM_CATEGORY_FOOD,
    ITEM_CATEGORY_WEAPON,
    ITEM_CATEGORY_ARMOR,
    ITEM_CATEGORY_POTION,
    ITEM_CATEGORY_MISC
]

const OPTION_LABEL = "label";
const OPTION_CATEGORY = "category";
const OPTION_QUALITY = "quality";
const OPTION_SELLIN = "sellIn";

class AddItemModel extends BaseComponent {

    constructor(props) {
        super(props);
        this._bind('hideModal', 'isConfigValueEqualTo', 'setConfigValue', 'createItem');
        this.state = {
            [OPTION_LABEL]: '',
            [OPTION_CATEGORY]: ITEM_CATEGORY_CONJURED,
            [OPTION_QUALITY]: 20,
            [OPTION_SELLIN]: 20
        }
    }

    hideModal() {
        hideModal();
    }

    isConfigValueEqualTo(value, configOption) {
        return this.state[configOption] === value;
    }

    setConfigValue(configOption, e) {
        this.state[configOption] = e.target.value;
    }

    setConfigValueNumber(configOption, e) {
        this.state[configOption] = parseInt(e.target.value);
    }

    createItem() {
        addItem(this.state).then(()=>{
            hideModal();
        });
    }

    render() {
        const self = this;

        const categoryOptions = [];

        ITEM_CATEGORIES.forEach((itemCategory) => {
            categoryOptions.push(<option key={`${OPTION_CATEGORY}-${itemCategory}`} value={itemCategory} selected={self.isConfigValueEqualTo(OPTION_CATEGORY, itemCategory)}>{itemCategory}</option>)
        })

        return (

            <Modal
                show={this.props.modal.show}
                aria-labelledby="ModalHeader"
                onHide={self.hideModal}
            >
                <Modal.Header closeButton>
                    <Modal.Title id='ModalHeader'>Configure KF360 KFLA Group Report</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form horizontal>
                        <FormGroup controlId="horizontalLabel">
                            <Col componentClass={ControlLabel} sm={6}>
                                <FontAwesome name="category" />Label
                                                </Col>
                            <Col sm={6}>
                                <FormControl componentClass="input" onChange={(e) => self.setConfigValue(OPTION_LABEL, e)} placeholder="Choose a dashing label..." style={{ maxWidth: '200px' }} />
                            </Col>
                        </FormGroup>
                        <FormGroup controlId="formHorizontalCategory">
                            <Col componentClass={ControlLabel} sm={6}>
                                <FontAwesome name="category" />Category
                                                </Col>
                            <Col sm={6}>
                                <FormControl componentClass="select" onChange={(e) => self.setConfigValue(OPTION_CATEGORY, e)} placeholder="Select a category for the item you are about to create" style={{ maxWidth: '200px' }}>
                                    {categoryOptions}
                                </FormControl>
                            </Col>
                        </FormGroup>
                        <FormGroup controlId="horizontalQuality">
                            <Col componentClass={ControlLabel} sm={6}>
                                <FontAwesome name="category" />Quality
                                                </Col>
                            <Col sm={6}>
                                <FormControl componentClass="input" type="number" onChange={(e) => self.setConfigValueNumber(OPTION_QUALITY, e)} placeholder="Choose the initial quality (Number greater than 0)" style={{ maxWidth: '200px' }} />
                            </Col>
                        </FormGroup>
                        <FormGroup controlId="horizontalQuality">
                            <Col componentClass={ControlLabel} sm={6}>
                                <FontAwesome name="category" />Sell In (Days)
                                                </Col>
                            <Col sm={6}>
                                <FormControl componentClass="input" type="number" onChange={(e) => self.setConfigValueNumber(OPTION_SELLIN, e)} placeholder="Days until it needs to be sold" style={{ maxWidth: '200px' }} />
                            </Col>
                        </FormGroup>
                    </Form>
                </Modal.Body>
                <Modal.Footer>
                    <Button bsStyle="success" onClick={self.createItem}><FontAwesome name="magic"/> Manifest It!</Button>&nbsp;<Button bsStyle="primary" onClick={self.hideModal}><FontAwesome name="thumbs-o-up"/> All Done</Button>
                </Modal.Footer>
            </Modal>)
    }
};

export default branch({
    modal: ['ui', 'modal']
}, AddItemModel)

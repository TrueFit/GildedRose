import React from 'react';
import {branch} from 'baobab-react/higher-order';

import { PageHeader } from 'react-bootstrap';
import {BootstrapTable, TableHeaderColumn} from 'react-bootstrap-table';

import BaseComponent from '../components/baseComponent';
import InventoryList from '../components/inventory/inventoryList';

class CurrentInventory extends BaseComponent {

    render() {
        return (
            <div>
                <PageHeader>Current Inventory <small>All items that can be safely sold by the Inn</small></PageHeader>
                <InventoryList inventory={this.props.inventory} deleteWarning/>
            </div>

        )
    }
}

export default branch({
    inventory: ['inventory', 'good']
}, CurrentInventory);
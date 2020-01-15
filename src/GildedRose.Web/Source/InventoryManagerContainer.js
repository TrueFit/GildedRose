import React from "react";
import InventoryTable from "./InventoryTable";
import CommandBar from "./CommandBar";
import * as api from "./api";
import AddFormContainer from "./AddFormContainer";
import { displaySort } from "./util";

class InventoryManagerContainer extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            inventory: [],
            filter: {}
        };

        this.addItem = this.addItem.bind(this);
        this.advanceToNextDay = this.advanceToNextDay.bind(this);
        this.filterByName = this.filterByName.bind(this);
        this.filterTrashOnly = this.filterTrashOnly.bind(this);
        this.listAll = this.listAll.bind(this);
        this.refresh = this.refresh.bind(this);
        this.removeItem = this.removeItem.bind(this);
        this.reset = this.reset.bind(this);
    }

    componentDidMount() {
        this.refresh();
    }

    addItem(name, category, quality, sellIn) {
        api.add(name, category, quality, sellIn).then(
            item => this.setState(prevState => {
                return {
                    inventory: [...prevState.inventory, item].sort(displaySort)
                };
            }),
            error => console.error(error)
        );
    }

    advanceToNextDay() {
        api.nextDay().then(
            inventory => this.setState({
                filter: {},
                inventory: [...inventory].sort(displaySort)
            }),
            error => console.error(error)
        );
    }

    filterByName(event) {
        const { value } = event.target;
        this.setState({
            filter: { byName: value }
        });
    }

    filterTrashOnly() {
        this.setState({
            filter: { trashOnly: true }
        });
    }

    listAll() {
        this.setState({
            filter: {}
        });
    }

    refresh() {
        api.getAll().then(
            inventory => this.setState({
                filter: {},
                inventory: [...inventory].sort(displaySort)
            }),
            error => console.error(error)
        );
    }

    removeItem(id) {
        api.removeItem(id).then(
            _ => this.setState(prevState => {
                return {
                    inventory: prevState.inventory.filter(item => item.id !== id).sort(displaySort)
                };
            }),
            error => console.error(error)
        );
    }

    reset() {
        if (window.confirm("This will reset the inventory to the state of the original inventory.txt file. Are you sure you want to do this?")) {
            api.reset().then(
                _ => this.refresh(),
                error => console.error(error)
            );
        }
    }

    render() {
        const { inventory, filter } = this.state;

        let temp = [];
        const distinctNames = inventory.reduce((accumulator, current) => {
            if (temp.indexOf(current.name.toUpperCase()) === -1) {
                temp.push(current.name.toUpperCase());
                accumulator.push(current.name);
            }
            return accumulator;
        }, []);

        temp = [];
        const distinctCategories = inventory.reduce((accumulator, current) => {
            if (temp.indexOf(current.category.toUpperCase()) === -1) {
                temp.push(current.category.toUpperCase());
                accumulator.push(current.category);
            }
            return accumulator;
        }, []);

        let filteredInventory;
        if (filter.trashOnly) {
            filteredInventory = inventory.filter(i => i.quality <= 0);
        } else if (filter.byName) {
            filteredInventory = inventory.filter(i => i.name === filter.byName);
        } else {
            filteredInventory = inventory;
        }

        return (
            <React.Fragment>
                <CommandBar
                    advanceToNextDay={this.advanceToNextDay}
                    filterByName={this.filterByName}
                    filterTrashOnly={this.filterTrashOnly}
                    itemNames={distinctNames}
                    listAllInventory={this.listAll}
                    selectedName={filter.byName || ""}
                />
                <AddFormContainer addItem={this.addItem} categories={distinctCategories} />
                <InventoryTable items={filteredInventory} removeItem={this.removeItem} />
                <hr />
                <button className="btn btn-danger" type="button" onClick={this.reset}>System reset</button>
            </React.Fragment>
        );
    }
}

export default InventoryManagerContainer;
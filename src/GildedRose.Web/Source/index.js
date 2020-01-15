import React from "react";
import ReactDOM from "react-dom";
import InventoryManagerContainer from "./InventoryManagerContainer";

function run(node) {
    ReactDOM.render(<InventoryManagerContainer />, node);
}

window.GILDED_ROSE = window.GILDED_ROSE || {};
window.GILDED_ROSE.run = run;
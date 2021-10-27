import React from 'react';
import {NavLink} from 'react-router-dom';
import {Navbar, Nav} from 'react-bootstrap';

export default (props) => {
    return (
        <Navbar bg="dark" expand="lg">
            <Nav>
                <Navbar.Toggle aria-controls="basic-navbar-nav" />
                <Navbar.Collapse id="basic-navbar-nav" />
                <NavLink className="d-inline p-2 bg-dark text-white" to="/">Home</NavLink>
                <NavLink className="d-inline p-2 bg-dark text-white" to="/Inventory">Inventory</NavLink>
                <NavLink className="d-inline p-2 bg-dark text-white" to="/Trash">Trash List</NavLink>
            </Nav>
        </Navbar>
    )
}
import React from 'react';
import {connect} from 'react-redux';

import {Navbar, Nav, NavItem, MenuItem, NavDropdown} from 'react-bootstrap';

class Header extends React.Component{
    render(){
        return(
            <Navbar inverse collapseOnSelect>
                <Navbar.Header>
                    <Navbar.Brand>
                    <a href="/">Gilded Rose</a>
                    </Navbar.Brand>
                    <Navbar.Toggle />
                </Navbar.Header>
                <Navbar.Collapse>
                    <Nav pullRight>
                    <NavItem eventKey={1} href="/item">
                        Select Item
                    </NavItem>
                    <NavItem eventKey={2} href="/itemslist">
                        List Items
                    </NavItem>
                    <NavItem eventKey={3} href="/trash">
                        Trash
                    </NavItem>
                    <NavItem eventKey={4} href="/endofday">
                        End Of Day
                    </NavItem>
                    </Nav>
                </Navbar.Collapse>
            </Navbar>
        );
    }
}

export default connect()(Header);
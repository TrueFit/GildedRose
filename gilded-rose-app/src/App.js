import React from 'react';
import { root } from 'baobab-react/higher-order';

import { Navbar, Nav, NavItem, NavDropdown, MenuItem, Button, Image } from 'react-bootstrap';
import { IndexLinkContainer } from 'react-router-bootstrap';
import FontAwesome from 'react-fontawesome';


import 'react-bootstrap-table/dist/react-bootstrap-table-all.min.css';
import './App.css';

import state from './state/state';

import BaseComponent from './components/baseComponent';
import Spinner from './components/spinner/spinner';
import NavBar from './components/nav/navBar';

class App extends BaseComponent {

  render() {
    return (
      <div>
        <Spinner />
        <NavBar/>
        <div className="container-fluid">
          {this.props.children}
        </div>
      </div>
    );
  }
}

const RootedApp = root(state, App);

export default RootedApp;

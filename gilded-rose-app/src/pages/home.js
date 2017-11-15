import React from 'react';
import BaseComponent from '../components/baseComponent';

import { Jumbotron, Button } from 'react-bootstrap';
import FontAwesome from 'react-fontawesome';

class HomePage extends BaseComponent {

    constructor(props) {
        super(props);
    }

    render() {
        return (
            <Jumbotron>
                <h1>Welcome To The Inn!</h1>
                <p>Allison, the gnomes have worked hard (and we have provided plenty of <FontAwesome name="beer"/> in the process) to present you with this portal for managing all the goods you offer to wary travellers. With great power comes great responsibility, shall the data put forth bring happiness to all!</p>
                <p><Button bsStyle="primary" href="/currentInventory">Check Thee Inventory</Button></p>
            </Jumbotron>
        )
    }
}

export default HomePage;
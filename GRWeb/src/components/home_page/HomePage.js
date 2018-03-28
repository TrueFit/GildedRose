import React from 'react';
import {connect} from 'react-redux';

import {Jumbotron} from 'react-bootstrap';


class HomePage extends React.Component{
    constructor(props){
        super(props);
    }
    render(){
        return(
            <div>
                <Jumbotron>
                    <div className="container vertical-container">
                    <h1>Gilded Rose</h1>
                    <h3>Invenotry Managment System</h3>
                    
                    <ul>
                        <li>
                           <span><a href="/item">Select Item</a> 
                           - Get Information about a particular Item.</span>                        
                        </li>  
                        <li>
                            <span><a href="/itemslist">List Items</a> 
                            - List all Items in Inventory.</span>
                        </li> 
                        <li>
                            <span><a href="/trash">Trash List</a> 
                            - List all Trash Items in Inventory.</span>
                         </li>
                         <li>   
                            <span><a href="/endofday">End Of Day</a>
                            - Preform the End Of Day Process.</span>
                        </li>
                    </ul>
                    </div>
                </Jumbotron>
            </div>
        );
    }
}

export default connect()(HomePage);
import React from 'react';
import {Jumbotron} from 'react-bootstrap';
import {Link} from 'react-router-dom';


export default class HomePage extends React.Component{
    render(){
        return(
            <div>
                <Jumbotron>
                    <div className="container vertical-container">
                    <h1>Gilded Rose</h1>
                    <h3>Invenotry Managment System</h3>
                    
                    <ul>
                        <li>
                           <span><Link to={"/item"}>Select Item</Link> 
                           - Get Information about a particular Item.</span>                        
                        </li>  
                        <li>
                            <span><Link to={"/itemslist"}>List Items</Link> 
                            - List all Items in Inventory.</span>
                        </li> 
                        <li>
                            <span><Link to={"/trash"}>Trash List</Link> 
                            - List all Items in Inventory.</span>
                         </li>
                         <li>   
                            <span><Link to={"/endofday"}>End Of Day</Link>
                            - Get Information about a particular Item.</span>
                        </li>
                    </ul>
                    </div>
                </Jumbotron>
            </div>
        );
    }
}
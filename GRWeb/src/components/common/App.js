// Handles the Templage used by every page.
import React from 'react';
import Header from './Header';

export default class AppPage extends React.Component{
    render(){
        return(
            <div id>
                <Header />
                <div className="container-fluid">
                {this.props.children}
                </div>
            </div>
        );
    }
}
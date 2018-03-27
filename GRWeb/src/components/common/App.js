// Handles the Templage used by every page.
import React from 'react';
import {connect} from 'react-redux';

import Header from './Header';

class AppPage extends React.Component{
    constructor(props){
        super(props);
    }
    
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

export default connect()(AppPage);
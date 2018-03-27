// Handles the Templage used by every page.
import React from 'react';
import {PropTypes} from 'prop-types';
import {connect} from 'react-redux';

import Header from './Header';
import LoadingDots from './LoadingDots';

class AppPage extends React.Component{
    constructor(props){
        super(props);
    }

    render(){
        let loading = this.props.loading;
        return(
            <div id>
                <Header />
                <div className="container-fluid">
                {this.props.children}
                {loading && <LoadingDots interval={100} dots={20} />}
                </div>
            </div>
        );
    }
}

AppPage.propTypes = {
    children: PropTypes.array.isRequired,
    loading: PropTypes.bool.isRequired
};

function mapStateToProps(state, ownProps){
    return {
        loading: state.apiCallsInProcess > 0
    };
}

export default connect(mapStateToProps)(AppPage);
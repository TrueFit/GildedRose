import React from 'react';
import {branch} from 'baobab-react/higher-order';
import BaseComponent from '../baseComponent';
import Loadable from 'react-loading-overlay';

class Spinner extends BaseComponent {

    render() {
        let spinnerDisplay = this.props.spinner.show ? 'block' : 'none';
        let spinnerText = this.props.spinner.text ? this.props.spinner.text : '';
        return (
            <Loadable
                active={this.props.spinner.show}
                spinner
                spinnerSize="100px"
                text={spinnerText}
                style={{ position: 'fixed', width: '100%', height: '100%', left: '0', top: '0', zIndex: 99999, display: spinnerDisplay }}
            >
            </Loadable>
        )
    }

}

export default branch({
    spinner: ['ui','spinner']
}, Spinner)
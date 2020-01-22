import { connect } from 'react-redux';

import * as actions from '../actions';
import App from './App';

const mapStateToProps = state => {
    return {
        inventory: state.inventory
    };
}

const mapDispatchToProps = dispatch => {
    return {
      handleGetAllItems: () => dispatch(actions.getAllItems()),
      handleGetTrash: () => dispatch(actions.getTrash()),
      handleAdvanceDay: () => dispatch(actions.advanceDay()),
    };
}

export default connect(mapStateToProps, mapDispatchToProps)(App);
import {combineReducers} from 'redux';
import items from './itemsReducer';
//import authors from './authorReducer';
//import ajaxCallsInProgress from './ajaxStatusReducer';

const rootReducer = combineReducers({
    itemsList: items
});

export default rootReducer;
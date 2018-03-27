import {combineReducers} from 'redux';
import itemsList from './itemsListReducer';
import trash from './trashReducer';
import item from './itemReducer';
//import authors from './authorReducer';
//import ajaxCallsInProgress from './ajaxStatusReducer';


// Rename the reducers so that I do not get a warning when setting initial state when creating store
const rootReducer = combineReducers({
    itemsList,
    trashList: trash,
    item
});

export default rootReducer;
import {combineReducers} from 'redux';
import itemsList from './itemsListReducer';
import trash from './trashReducer';
import item from './itemReducer';
import apiCallStatus from './apiCallStatusReducer';


// Rename the reducers so that I do not get a warning when setting initial state when creating store
const rootReducer = combineReducers({
    itemsList,
    trashList: trash,
    item,
    apiCallsInProcess: apiCallStatus
});

export default rootReducer;
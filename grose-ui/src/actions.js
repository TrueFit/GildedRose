export const GET_ALL_ITEMS = "GET_ALL_ITEMS";
export const GET_TRASH = "GET_TRASH";
export const ADVANCE_DAY = "ADVANCE_DAY";

export const getAllItems = () => {
    return {
        type: GET_ALL_ITEMS
    };
}

export const getTrash = () => {
    return {
        type: GET_TRASH
    }
}

export const advanceDay = () => {
    return {
        type: ADVANCE_DAY
    }
}

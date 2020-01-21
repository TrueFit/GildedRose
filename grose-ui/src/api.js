import wretch from 'wretch';

const API_URL = process.env.REACT_APP_API_URL;

export const getAllItems = async () => {
    const url = `${API_URL}/items`;
    return await wretch(url).get().json();
}

export const getTrash = async () => {
    const url = `${API_URL}/items?trash`;
    return await wretch(url).get().json();
}

export const postAdvanceDay = async () => {
    const url = `${API_URL}/nextday`;
    return await wretch(url).post(null).json();
}
import Baobab from 'baobab';

export default new Baobab({
    ui : {
        spinner: {
            show:false,
            text:""
        },
        modal: {
            show:false
        }
    },
    inventory: {
        good:[],
        bad:[]
    }
});
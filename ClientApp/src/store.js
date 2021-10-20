import { configure, observable } from "mobx";

configure({
  enforceActions: "never",
});

const store = observable({
  list: {
    isValid: false,
    items: [],
  },
  trashed: {
    isValid: false,
    items: [],
  },
  item: {
    isValid: false,
  },
});

export { store };

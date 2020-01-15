import PropTypes from "prop-types";

export const inventoryItemType = PropTypes.shape({
    id: PropTypes.string.isRequired,
    name: PropTypes.string.isRequired,
    category: PropTypes.string.isRequired,
    sellIn: PropTypes.number,
    quality: PropTypes.number.isRequired
});

export const inventoryCollectionType = PropTypes.arrayOf(inventoryItemType);
import React from 'react';
import InventoryItem from './InventoryItem';
import '../styles/App.css';

const App = props => {
  const {
    inventory,
    handleGetAllItems,
    handleGetTrash,
    handleAdvanceDay,
  } = props;
  return (
    <div className="App">
      <h1>Guilded Rose Inventory</h1>
      <div className="Controls">
        <button 
          type="button"
          onClick={() => handleGetAllItems()}
        >View All Items</button>
        <button
          type="button"
          onClick={() => handleGetTrash()}
        >View Trash</button>
        <button
          type="button"
          onClick={() => handleAdvanceDay()}
        >Advance to the Next Day</button>
      </div>
      <div className="InventoryView">
        {inventory.length > 0 && (
          <div className="Heading">
            <div>Name</div>
            <div>Category</div>
            <div>Sell In</div>
            <div>Quality</div>
          </div>
        )}
        {inventory.map((item, i) => (
            <InventoryItem
              key={i}
              name={item.name}
              category={item.category}
              sellIn={item.sellIn}
              quality={item.quality}
            />
        ))}
      </div>
    </div>
  );
}

export default App;

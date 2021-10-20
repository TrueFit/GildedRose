import React, { useEffect } from "react";
import { observer } from "mobx-react-lite";
import Button from "reactstrap/lib/Button";
import { useHistory } from "react-router";

const Trashed = observer(({ trashed }) => {
  const history = useHistory();

  useEffect(() => {
    if (!trashed.isValid) {
      fetch("inventory/trashed")
        .then((response) => response.json())
        .then((items) => {
          trashed.items = items;
          trashed.isValid = true;
        });
    }
  }, [trashed.isValid]);

  const goToDetail = (item) => {
    history.push(`/goods/${item.name}`);
  };

  if (trashed.items.length === 0) {
    return <div className="trash-empty">Trash is Empty</div>;
  }

  return (
    <div className="items-container">
      {trashed.items.map((item) => (
        <div className="item" key={item.name}>
          <div className="item-field">
            <span className="item-field-title">Name:</span>
            <span className="item-field-value">{item.name}</span>
          </div>
          <div className="item-field">
            <span className="item-field-title">Category:</span>
            <span className="item-field-value">{item.category}</span>
          </div>
          <div className="item-field">
            <span className="item-field-title">Q:</span>
            <span className="item-field-value">{item.quality}</span>
            <span className="item-field-title">SI:</span>
            <span className="item-field-value">{item.sellIn}</span>
          </div>
          <div className="item-button">
            <Button color="primary" size="sm" onClick={() => goToDetail(item)}>
              Show Details
            </Button>
          </div>
        </div>
      ))}
    </div>
  );
});

export { Trashed };

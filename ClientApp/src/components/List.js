import React, { useEffect, useState } from "react";
import { observer } from "mobx-react-lite";
import Button from "reactstrap/lib/Button";
import { useHistory } from "react-router";

const List = observer(({ list }) => {
  const history = useHistory();

  useEffect(() => {
    if (!list.isValid) {
      fetch("inventory/goods")
        .then((response) => response.json())
        .then((items) => {
          list.items = items;
          list.isValid = true;
        });
    }
  }, [list.isValid]);

  const goToDetail = (item) => {
    history.push(`/goods/${item.name}`);
  };

  return (
    <div className="items-container">
      {list.items.map((item) => (
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

export { List };

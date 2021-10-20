import React, { useState, useEffect } from "react";
import { observer } from "mobx-react-lite";
import { useParams } from "react-router";

const Detail = observer(({ item }) => {
  const [theItem, setTheItem] = useState();
  const { name } = useParams();

  useEffect(() => {
    if (!item.isValid) {
      fetch(`inventory/goods/${name}`)
        .then((response) => response.json())
        .then((theItem) => {
          setTheItem(theItem);
          item.isValid = true;
        });
    }
  }, [item.isValid]);

  useEffect(() => {
    // component unmount
    return function () {
      item.isValid = false;
    };
  });

  if (!theItem) {
    return "";
  }

  return (
    <div className="detail">
      <div className="detail-field">
        <span className="detail-field-title">Name</span>
        <span className="detail-field-value">{theItem.name}</span>
      </div>
      <div className="detail-field">
        <span className="detail-field-title">Category</span>
        <span className="detail-field-value">{theItem.category}</span>
      </div>
      <div className="detail-field">
        <span className="detail-field-title">Quality</span>
        <span className="detail-field-value">{theItem.quality}</span>
      </div>
      <div className="detail-field">
        <span className="detail-field-title">Sell in</span>
        <span className="detail-field-value">{theItem.sellIn}</span>
      </div>
    </div>
  );
});

export { Detail };

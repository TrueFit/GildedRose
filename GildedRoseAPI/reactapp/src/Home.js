import React, { useState } from 'react';
import { Button } from 'react-bootstrap';

export default (props) => {
    const [message, setMessage] = useState("");

    const ResetInventory = () => {
        fetch('https://localhost:5001/api/Item/ResetItems', {method:'POST', headers:{'Accept': 'application/json','Content-Type': 'application/json'}})
        .then(response => response.json())
        .then(result => {
            setMessage(result.message);
        });
    }

    return (
        <div>
            <p>Welcome to the Gilded Rose!</p>
            <Button variant="danger" onClick={()=>ResetInventory()}>Reset Inventory</Button>
            <p>{message}</p>
        </div>
    )
}
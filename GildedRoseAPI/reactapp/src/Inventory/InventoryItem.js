import React, { useState, useEffect } from 'react';
import { Table } from 'react-bootstrap';

export default (props) => {

    const [item, setItem] = useState([]);
    const [message, setMessage] = useState("");

    useEffect(() => {
        LoadItem(props.itemName);
    }, [props.itemName])

    const LoadItem = (itemName) => {
        fetch('https://localhost:5001/api/Item/GetItem', {
                method:'POST', 
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }, 
                body: JSON.stringify({name: itemName})
            })
        .then(response => response.json())
        .then(result => {
            if (result.success) {
                //console.log(result.item);
                setItem(result.item);
                setMessage("");
            } else {
                setMessage(result.message);
            }
        });
    }

    return (
        <div>
            <h3>{item.name}</h3>
            <Table className="mt-4" striped bordered hover size="sm">
                    <thead>
                        <tr>
                            <th>Category</th>
                            <th>Sell In</th>
                            <th>Quality</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>{item.category}</td>
                            <td>{item.sellIn}</td>
                            <td>{item.quality}</td>
                        </tr>
                    </tbody>
            </Table>
            {message}
        </div>
    )
}
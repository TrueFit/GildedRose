import React, { useState, useEffect } from 'react';
import { Table, Button } from 'react-bootstrap';

export default (props) => {

    const [inventory, setInventory] = useState([]);
    const [message, setMessage] = useState("");

    useEffect(() => {
        LoadInventoryToTrash();
    }, [])

    const LoadInventoryToTrash = () => {
        fetch('https://localhost:5001/api/Item/GetItemsToTrash', {method:'GET', headers:{'Accept': 'application/json','Content-Type': 'application/json'}})
        .then(response => response.json())
        .then(result => {
            if (result.success) {
                //console.log(result.items);
                setInventory(result.items);
                setMessage("");
            } else {
                setMessage(result.message);
            }
        });
    }

    const TrashAll = () => {
        fetch('https://localhost:5001/api/Item/TrashAll', {method:'POST', headers:{'Accept': 'application/json','Content-Type': 'application/json'}})
        .then(response => response.json())
        .then(result => {
            //console.log(result);
            if (result.success) {
                LoadInventoryToTrash();
            } else {
                setMessage(result.message);
            }
        });
    }

    return (
        <div>
            <p>Inventory List</p>
            <Table className="mt-4" striped bordered hover size="sm">
                    <thead>
                        <tr>
                            {/*<th>ID</th>*/}
                            <th>Name</th>
                            <th>Category</th>
                            <th>Sell In</th>
                            <th>Quality</th>
                        </tr>
                    </thead>
                    <tbody>
                        {inventory && inventory.map(item => <tr>
                            {/*<td>{item.itemID}</td>*/}
                            <td>{item.name}</td>
                            <td>{item.category}</td>
                            <td>{item.sellIn}</td>
                            <td>{item.quality}</td>
                        </tr>)}
                    </tbody>
            </Table>
            <Button variant="primary" onClick={()=>TrashAll()}>Trash</Button>
            {message}
        </div>
    )
}
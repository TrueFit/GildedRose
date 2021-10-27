import React, { useState, useEffect } from 'react';
import { useHistory } from 'react-router-dom';
import { Table, Button } from 'react-bootstrap';

export default (props) => {

    const [inventory, setInventory] = useState([]);
    const [message, setMessage] = useState("");

    useEffect(() => {
        LoadInventory();
    }, [])

    const LoadInventory = () => {
        fetch('https://localhost:5001/api/Item/GetItems', {method:'GET', headers:{'Accept': 'application/json','Content-Type': 'application/json'}})
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

    const ProgressToNextDay = () => {
        fetch('https://localhost:5001/api/Item/ProgressToNextDay', {method:'POST', headers:{'Accept': 'application/json','Content-Type': 'application/json'}})
        .then(response => response.json())
        .then(result => {
            console.log(result);
            if (result.success) {
                LoadInventory();
            } else {
                setMessage(result.message);
            }
        });
    }

    const history = useHistory();
    const NavigateToItem = (itemName) => {
        history.push(`/Item/${itemName}`);
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
                        {inventory && inventory.map(item => <tr onClick={() => NavigateToItem(item.name)}>
                            {/*<td>{item.itemID}</td>*/}
                            <td>{item.name}</td>
                            <td>{item.category}</td>
                            <td>{item.sellIn}</td>
                            <td>{item.quality}</td>
                        </tr>)}
                    </tbody>
            </Table>
            <Button variant="primary" onClick={()=>ProgressToNextDay()}>Progress To Next Day</Button>
            {message}
        </div>
    )
}
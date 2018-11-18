import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { Router } from '@angular/router';
import { Inventory } from '../../models/inventory';

@Component({
    selector: 'inventory',
    templateUrl: './inventory.component.html',
    styleUrls: ['./inventory.component.css']
})
export class InventoryComponent {
    public inventories: Inventory[];

    constructor(http: Http, @Inject('BASE_URL') baseUrl: string, private router: Router) {
        http.get(baseUrl + '/api/v1/inventories').subscribe(result => {

            this.inventories = result.json() as Inventory[];

        }, error => console.error(error));
    }

    openInventory(inventoryId: number) {
        this.router.navigate(['/items', inventoryId]);
    }    

}

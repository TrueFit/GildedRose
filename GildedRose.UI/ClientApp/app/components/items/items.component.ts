import { Component, Inject, OnInit, OnDestroy } from '@angular/core';
import { Http } from '@angular/http';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { Item } from '../../models/item';
import { Quality } from '../../models/quality';
import { Inventory } from '../../models/inventory';

@Component({
    selector: 'items',
    templateUrl: './items.component.html',
    styleUrls: ['./items.component.css']
})
export class ItemsComponent implements OnInit, OnDestroy {
    inventoryId: number;
    public inventory: Inventory;
    public itemName: string = '';
    public settingUp: boolean;
    private sub: any;
    public items: Item[];

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, private route: ActivatedRoute, private router: Router) {}

    ngOnInit() {
        this.searchItems();
        this.getInventory();
    }

    searchItems() {
        this.sub = this.route.params.subscribe(params => {
            this.inventoryId = +params['inventoryId'];
            let itemSearchUri = this.baseUrl + '/api/v1/items/search?inventoryId=' + this.inventoryId + '&itemName=' + this.itemName;
            this.http.get(itemSearchUri).subscribe(result => {

                this.items = result.json() as Item[];

            }, error => console.error(error));
        });
    }

    getInventory() {
        let inventoryUri = this.baseUrl + '/api/v1/inventories/' + this.inventoryId;
        this.http.get(inventoryUri).subscribe(result => {

            this.inventory = result.json() as Inventory;

        }, error => console.error(error));
    }

    openInventory() {
        this.router.navigate(['/inventory']);
    } 

    advanceDay() {
        this.settingUp = true;
        let advanceInventoryDayUri = this.baseUrl + '/api/v1/inventories/' + this.inventoryId + '/advance';
        var result = this.http.post(advanceInventoryDayUri,{})
            .subscribe(
                (val) => {
                    this.searchItems();
                    this.getInventory();
                    this.settingUp = false;
                },
                response => {
                    console.log("Failed to add inventory", response);
                },
                () => {
                    console.log("Complete inventory add");
                });
    }

    updateInventoryDate() {

    }

    ngOnDestroy() {
        this.sub.unsubscribe();
    }
  
}





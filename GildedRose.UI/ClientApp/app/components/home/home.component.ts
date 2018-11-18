import { Component, Inject, OnInit } from '@angular/core';
import { Http, RequestOptions, Headers } from '@angular/http';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { Inventory } from '../../models/inventory'

@Component({
    selector: 'home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css']
})
export class HomeComponent {
    public inventory: Inventory;
    public name: string;
    public owner: string;
    public settingUp: boolean = false;
    private sub: any;
    private defaultInventoryUri: string = '/api/v1/inventories/default'

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, private router: Router) { }

    purchaseInventory() {

        this.settingUp = true;

        let inv = new Inventory();
        inv.name = this.name;
        inv.owner = this.owner;

        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });

        let createInventoryUri = this.baseUrl + this.defaultInventoryUri;
        var result = this.http.post(createInventoryUri,
            {
                "name": this.name,
                "owner": this.owner
            })
            .subscribe(
                (val) => {
                    this.openInventory();
                },
                response => {
                    console.log("Failed to add inventory", response);
                    this.settingUp = false;
                },
                () => {
                    console.log("Complete inventory add");
                });
    }

    openInventory() {
        let that = this;
        setTimeout(function () {
            that.router.navigate(['/inventory']);
        }, 1000);
    }
}

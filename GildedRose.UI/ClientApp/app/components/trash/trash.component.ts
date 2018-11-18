import { Component, Inject, OnInit, OnDestroy } from '@angular/core';
import { Http } from '@angular/http';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { Item } from '../../models/item';
import { Quality } from '../../models/quality';

@Component({
    selector: 'trash',
    templateUrl: './trash.component.html',
    styleUrls: ['./trash.component.css']
})
export class TrashComponent implements OnInit, OnDestroy {
    private sub: any;
    public settingUp: boolean = false;
    public items: Item[];

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, private route: ActivatedRoute, private router: Router) {}

    ngOnInit() {
        this.searchItems();
    }

    searchItems() {
        this.sub = this.route.params.subscribe(params => {
            let trashUri = this.baseUrl + '/api/v1/items/trash';
            this.http.get(trashUri).subscribe(result => {

                this.items = result.json() as Item[];

            }, error => console.error(error));
        });
    }

    deleteItem(itemId: number) {
        this.settingUp = true;
        let deleteItemUri = this.baseUrl + '/api/v1/items/' + itemId;
        var result = this.http.delete(deleteItemUri, {})
            .subscribe(
                (val) => {
                    this.searchItems();
                    this.settingUp = false;
                },
                response => {
                    console.log("Failed to delete item", response);
                },
                () => {
                    console.log("Completed item deletion");
                });
    }
    
    ngOnDestroy() {
        this.sub.unsubscribe();
    }
  
}





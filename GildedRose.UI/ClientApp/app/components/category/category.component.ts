import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { Category } from '../../models/category'

@Component({
    selector: 'category',
    templateUrl: './category.component.html',
    styleUrls: ['./category.component.css']
})

export class CategoryComponent {
    public categories: Category[];

    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        http.get(baseUrl + '/api/v1/categories').subscribe(result => {

            this.categories = result.json() as Category[];
            console.log(this.categories);

        }, error => console.error(error));
    }
}
import { Component, OnInit } from "@angular/core";

import { Product } from "./product";
import { ProductService } from "./product.service";

@Component({
    templateUrl: "./product-list.component.html"
})
export class ProductListComponent implements OnInit {
    constructor(private productService: ProductService) {
    }

    ngOnInit() {
        this.getProducts();
    }

    // Public properties
    products: Product[] = [];
    messages: string[] = [];
    searchEntity: string;

    private getProducts() {
        this.productService.getProducts()
            .subscribe(products => this.products = products,
            errors => this.handleErrors(errors));
    }

    search() {
        this.productService.search(this.searchEntity)
            .subscribe(products => this.products = products,
            errors => this.handleErrors(errors));
    }

    getAll() {
        this.searchEntity = "";
        this.getProducts();
    }

    endDay() {
        this.searchEntity = "";
        this.productService.endDay()
            .subscribe(products => this.products = products,
            errors => this.handleErrors(errors));
    }

    getZeroItems() {
        this.searchEntity = "";
        this.productService.getZeroItems()
            .subscribe(products => this.products = products,
            errors => this.handleErrors(errors));
    }

    resetInventory() {
        this.searchEntity = "";
        this.productService.resetInventory()
            .subscribe(products => this.products = products,
            errors => this.handleErrors(errors));
    }

    private handleErrors(errors: any) {
        this.messages = [];

        for (let msg of errors) {
            this.messages.push(msg);
        }
    }
}
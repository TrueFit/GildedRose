"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var product_service_1 = require("./product.service");
var ProductListComponent = (function () {
    function ProductListComponent(productService) {
        this.productService = productService;
        // Public properties
        this.products = [];
        this.messages = [];
    }
    ProductListComponent.prototype.ngOnInit = function () {
        this.getProducts();
    };
    ProductListComponent.prototype.getProducts = function () {
        var _this = this;
        this.productService.getProducts()
            .subscribe(function (products) { return _this.products = products; }, function (errors) { return _this.handleErrors(errors); });
    };
    ProductListComponent.prototype.search = function () {
        var _this = this;
        this.productService.search(this.searchEntity)
            .subscribe(function (products) { return _this.products = products; }, function (errors) { return _this.handleErrors(errors); });
    };
    ProductListComponent.prototype.getAll = function () {
        this.searchEntity = "";
        this.getProducts();
    };
    ProductListComponent.prototype.endDay = function () {
        var _this = this;
        this.searchEntity = "";
        this.productService.endDay()
            .subscribe(function (products) { return _this.products = products; }, function (errors) { return _this.handleErrors(errors); });
    };
    ProductListComponent.prototype.getZeroItems = function () {
        var _this = this;
        this.searchEntity = "";
        this.productService.getZeroItems()
            .subscribe(function (products) { return _this.products = products; }, function (errors) { return _this.handleErrors(errors); });
    };
    ProductListComponent.prototype.resetInventory = function () {
        var _this = this;
        this.searchEntity = "";
        this.productService.resetInventory()
            .subscribe(function (products) { return _this.products = products; }, function (errors) { return _this.handleErrors(errors); });
    };
    ProductListComponent.prototype.handleErrors = function (errors) {
        this.messages = [];
        for (var _i = 0, errors_1 = errors; _i < errors_1.length; _i++) {
            var msg = errors_1[_i];
            this.messages.push(msg);
        }
    };
    return ProductListComponent;
}());
ProductListComponent = __decorate([
    core_1.Component({
        templateUrl: "./product-list.component.html"
    }),
    __metadata("design:paramtypes", [product_service_1.ProductService])
], ProductListComponent);
exports.ProductListComponent = ProductListComponent;
//# sourceMappingURL=product-list.component.js.map
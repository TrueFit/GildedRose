import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
  import { Observable } from 'rxjs/Observable';
  import 'rxjs/add/operator/map';
  import 'rxjs/add/operator/catch';
  import 'rxjs/add/observable/throw';

  import { Product } from "./product";

  @Injectable()
  export class ProductService {
      private url = "/api/productApi";
      private dayUrl = "/api/dayApi";

    constructor(private http: Http) {
    }

    getProducts(): Observable<Product[]> {
      return this.http.get(this.url)
        .map(this.extractData)
        .catch(this.handleErrors);
    }

    search(searchEntity: string): Observable<Product[]> {
        return this.http.get(this.url + "/Search/" + searchEntity)
            .map(this.extractData)
            .catch(this.handleErrors);
    }

    endDay(): Observable<Product[]> {
        return this.http.get(this.dayUrl + "/EndDay")
            .map(this.extractData)
            .catch(this.handleErrors);
    }

    getZeroItems(): Observable<Product[]> {
        return this.http.get(this.url + "/Zero")
            .map(this.extractData)
            .catch(this.handleErrors);
    }

    resetInventory(): Observable<Product[]> {
        return this.http.get(this.url + "/Reset")
            .map(this.extractData)
            .catch(this.handleErrors);
    }

  private extractData(res: Response) {
    let body = res.json();
    return body || {};
    }

  private handleErrors(error: any): Observable<any> {
    let errors: string[] = [];

    switch (error.status) {
      case 400: // Bad Request
        let err = error.json();
        if (err.message) {
          errors.push(err.message);
        }
        else {
          errors.push("An Unknown error occurred.");
        }
        break;

      case 404: // Not Found
        errors.push("No Product Data Is Available.");
        break;

      case 500: // Internal Error
        errors.push(error.json().exceptionMessage);
        break;

      default:
        errors.push("Status: " + error.status
          + " - Error Message: "
          + error.statusText);
        break;
    };

    console.error('An error occurred', errors);

    return Observable.throw(errors);
  }
}

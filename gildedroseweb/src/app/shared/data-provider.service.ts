import {Observable} from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';
import { catchError, map, tap } from 'rxjs/operators';

import {FormGroup} from '@angular/forms';
import {HttpClient, HttpErrorResponse, HttpHeaders} from '@angular/common/http';

import {environment} from '../../environments/environment';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

export abstract class DataProviderService<T> {
  protected resourceRoot = '';

  constructor(protected httpClient: HttpClient) {}

  list(params?: object): Observable<T[]> {
    let url = this.buildUrl(params);
    return this.httpClient.get<T[]>(url);
  }

  retrieve(id: number, params?: object): Observable<T> {
    let reqParams = params || {};
    reqParams['id'] = id;
    let url = this.buildUrl(reqParams);

    return this.httpClient.get<T>(url);
  }

  create(model: T, params?: object): Observable<T> {
    let url = this.buildUrl(params);

    return this.httpClient.post<T>(url, model, httpOptions)
      .pipe(
        catchError(this.handleError<T>('Error creating object!'))
      );
  }

  save(id: number | string, data?: T, params?: object): Observable<T> {
    let reqParams = params || {};
    reqParams['id'] = id;
    let url = this.buildUrl(reqParams);


    return this.httpClient.put<T>(url, data, httpOptions).pipe(
      catchError(this.handleError<any>('Error updating object!'))
    );
  }

  destroy(id: number, params?: object): Observable<T> {
    let reqParams = params || {};
    reqParams['id'] = id;
    let url = this.buildUrl(reqParams);

    return this.httpClient.delete<T>(url, httpOptions).pipe(
      catchError(this.handleError<T>('Error deleting object!'))
    );
  }

  protected makeRequest(method: string, data?: object, params?: object, targetUrl?: string) {
    const url = this.buildUrl(params, targetUrl);
    const headers = new HttpHeaders();  // Not setting anything for now.

    return this.httpClient.request(method, url, httpOptions);
  }

  mapErrorsToFormControls(errorResponse: HttpErrorResponse, form: FormGroup) {
    for (const error of errorResponse.error.errors) {
      const attribute = error.source.pointer.substr(error.source.pointer.lastIndexOf('/') + 1);
      const control = form.controls[attribute];

      if (control) {
        control.setErrors({'serverError': error.detail});
      }
    }
  }

  private buildUrl(params?: object, targetUrl?: string): string {
    params = params ? Object.assign({}, params) : {};

    let url = environment.apiRootURL + (targetUrl ? targetUrl : this.resourceRoot);
    let start = url.indexOf(':', this.nthIndex('/', url, 3)); // could make this smarter.

    // find and replace all url arguments
    while (start >= 0) {
      const end = url.indexOf('/', start);
      let replace = url.substring(start, end >= 0 ? end : url.length);
      const param = params[replace.substring(1)];

      if (param) {
        delete params[replace.substr(1)];
      } else if (end >= 0) {
        replace += '/';
      }

      url = url.replace(replace, param ? param : '');
      start = url.indexOf(':', start);
    }

    // add query string params
    let first = true;

    for (const key in params) {
      if (!params.hasOwnProperty(key)) {
          continue;
      }

      let prepend = '&';

      if (first) {
        first = false;
        prepend = '?';
      }

      url += (prepend + key + '=' + params[key]);
    }

    return url;
  }


  private nthIndex(needle: string, haystack: string, occurence: number) {
    // TODO: put this someplace else someday.
    const l = haystack.length;
    let i = -1;

    while (occurence-- && i++ < l) {
        i = haystack.indexOf(needle, i);

        if (i < 0) {
            break;
        }
    }

    return i;
  }

  /**
   * Handle Http operation that failed.
   * Let the app continue.
   * @param operation - name of the operation that failed
   * @param result - optional value to return as the observable result
   */
  private handleError<T> (operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {

      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }
}

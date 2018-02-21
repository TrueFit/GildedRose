import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';

import {DataProviderService} from '../../shared/data-provider-service';

@Injectable()
export class ItemDataProvider extends DataProviderService<Item> {
    resourceRoot = '/api/items/:id/';

    constructor(httpClient: HttpClient) {
        super(httpClient);
    }
}

import {NgModule} from '@angular/core';
import {RouterModule} from '@angular/router';

import {ItemListPageComponent} from './item-list-page/item-list-page.component';
import {ItemDetailComponent} from './item-detail/item-detail.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                component: ItemListPageComponent
            },
            {
                path: ':id/detail',
                component: ItemDetailComponent
            },
        ])
    ],
    exports: [
        RouterModule
    ]
})
export class ItemRoutingModule {}

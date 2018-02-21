import { NgModule } from '@angular/core';

import { SharedModule } from '../../shared/shared.module';

import { ItemRoutingModule } from './item-routing.module';
import { ItemDataProvider } from './item-data-provider.service';
import { ItemListPageComponent } from './item-list-page/item-list-page.component';
import { ItemDetailComponent } from './item-detail/item-detail.component'

@NgModule({
    imports: [
      SharedModule,
      ItemRoutingModule,
    ],
    declarations: [
      ItemListPageComponent,
      ItemDetailComponent,
      ItemDetailComponent,
    ],
    providers: [ItemDataProvider]
})
export class ItemModule {
}

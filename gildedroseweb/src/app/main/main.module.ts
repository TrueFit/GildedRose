import { NgModule } from '@angular/core';

import { SharedModule } from '../shared/shared.module';

import { MainRoutingModule } from './main-routing.module';
import { LayoutComponent } from './layout/layout.component';


@NgModule({
  imports: [
    SharedModule,
    MainRoutingModule,
  ],
  declarations: [
    LayoutComponent
  ]
})
export class MainModule {
}

import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { LayoutComponent } from './layout/layout.component';


@NgModule({
  imports: [
    RouterModule.forChild([
      {
        path: 'gr',
        component: LayoutComponent,
        children: [
          {
            path: '',
            redirectTo: 'items',
            pathMatch: 'full'
          },
          {
            path: 'items',
            loadChildren: './item/item.module#ItemModule'
          },
        ]
      },
    ])
  ],
  exports: [
    RouterModule
  ]
})
export class MainRoutingModule {
}

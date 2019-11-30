import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {HomePageComponent} from './home-page/home-page.component';
import {PosPageComponent} from './pos-page/pos-page.component';
import {InventoryPageComponent} from './inventory-page/inventory-page.component';
import {ReportsPageComponent} from './reports-page/reports-page.component';
import {NotFoundPageComponent} from './not-found-page/not-found-page.component';

const routes: Routes = [
  {path: 'home', component: HomePageComponent},
  {path: 'point-of-sale', component: PosPageComponent},
  {path: 'inventory', component: InventoryPageComponent},
  {path: 'reports', component: ReportsPageComponent},
  {path: '', redirectTo: '/home', pathMatch: 'full'},
  {path: '**', component: NotFoundPageComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}

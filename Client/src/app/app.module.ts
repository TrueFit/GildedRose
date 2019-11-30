import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {HeaderComponent} from './header/header.component';
import {HomePageComponent} from './home-page/home-page.component';
import {PosPageComponent} from './pos-page/pos-page.component';
import {InventoryPageComponent} from './inventory-page/inventory-page.component';
import {ReportsPageComponent} from './reports-page/reports-page.component';
import {NotFoundPageComponent} from './not-found-page/not-found-page.component';
import {HttpClientModule} from '@angular/common/http';
import {FormsModule} from '@angular/forms';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    HomePageComponent,
    PosPageComponent,
    InventoryPageComponent,
    ReportsPageComponent,
    NotFoundPageComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
}

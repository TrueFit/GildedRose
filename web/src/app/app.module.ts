import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { InventoryTableComponent } from './inventory-table/inventory-table.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HeaderComponent } from './header/header.component';
import {MatToolbarModule} from "@angular/material/toolbar";
import {MatIconModule} from "@angular/material/icon";
import {MatButtonModule} from "@angular/material/button";
import {MatTableModule} from "@angular/material/table";
import {HttpClientModule} from "@angular/common/http";
import {MatSortModule} from "@angular/material/sort";
import { WindowProvider } from './shared/providers';
import {MatTooltipModule} from "@angular/material/tooltip";
import { FileUploadComponent } from './file-upload/file-upload.component';
import {MatDialogModule} from "@angular/material/dialog";
import { FooterComponent } from './footer/footer.component';
import {ValueChangedPipe, ColorPipe} from "./shared/pipes";

@NgModule({
	declarations: [
		AppComponent,
		InventoryTableComponent,
		HeaderComponent,
		FooterComponent,
		FileUploadComponent,
		ValueChangedPipe,
		ColorPipe
	],
	imports: [
		BrowserModule,
		BrowserAnimationsModule,
		HttpClientModule,
		MatToolbarModule,
		MatIconModule,
		MatButtonModule,
		MatTableModule,
		MatSortModule,
		MatTooltipModule,
		MatDialogModule
	],
  providers: [WindowProvider],
  bootstrap: [AppComponent]
})
export class AppModule { }

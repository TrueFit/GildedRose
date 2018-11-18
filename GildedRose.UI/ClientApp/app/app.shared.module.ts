import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { CategoryComponent } from './components/category/category.component';
import { InventoryComponent } from './components/inventory/inventory.component';
import { ItemsComponent } from './components/items/items.component';
import { TrashComponent } from './components/trash/trash.component';
import { LoadingComponent } from './components/loading/loading.component';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        CategoryComponent,
        InventoryComponent,
        ItemsComponent,
        HomeComponent,
        TrashComponent,
        LoadingComponent
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'category', component: CategoryComponent },
            { path: 'inventory', component: InventoryComponent },
            { path: 'trash', component: TrashComponent },
            { path: 'items/:inventoryId', component: ItemsComponent },
            { path: '**', redirectTo: 'home' }
        ])
    ]
})
export class AppModuleShared {
}

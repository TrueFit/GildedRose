import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-inventory-history',
  templateUrl: './inventory-history.component.html'
})
export class InventoryHistoryComponent {
  public inventoryHistory: InventoryHistory[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<InventoryHistory[]>(baseUrl + 'api/inventory/history').subscribe(result => {
      this.inventoryHistory = result;
    }, error => console.error(error));
  }
}

interface InventoryHistory {
  name: string;
  category: string;
  sellIn: number;
  quality: number;
  inventoryAddedDate: string;
  lastModifiedDate: string;
  action: string;
}

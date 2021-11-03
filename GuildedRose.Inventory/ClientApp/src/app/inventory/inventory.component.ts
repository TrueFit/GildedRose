import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-inventory',
  templateUrl: './inventory.component.html',
})
export class InventoryComponent {
  public inventoryitems: InventoryItem[];
  public http: HttpClient;
  public baseUrl: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
    http.get<InventoryItem[]>(baseUrl + 'inventory').subscribe(result => {
      this.inventoryitems = result;
    }, error => console.error(error));
  }

  public incrementDay() {
    this.http.post<InventoryItem[]>(this.baseUrl + 'inventory/PostIncrementDay', '').subscribe(result => {
      this.inventoryitems = result;
    }, error => console.error(error));
  }
}

interface InventoryItem {
  name: string;
  category: string;
  sellIn: number;
  quality: number;
}

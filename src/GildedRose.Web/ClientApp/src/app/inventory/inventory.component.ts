import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-inventory',
  templateUrl: './inventory.component.html',
  styleUrls: ['./inventory.component.css']
})
export class InventoryComponent {
  public inventory: Inventory[];
  public showNofication: boolean;
  public showTrashOnly: boolean = false;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    this.loadInventory();
  }

  loadInventory() {
    let url = this.baseUrl + 'api/inventory' + (this.showTrashOnly ? '/trash' : '');
    this.http.get<Inventory[]>(url).subscribe(result => {
      this.inventory = result;
    }, error => console.error(error));
  }

  performAging() {
    this.http.post<any>(this.baseUrl + 'api/inventory/aging', null).subscribe(result => {
      this.loadInventory();
      this.showNofication = true;
    }, error => console.error(error));
  }

  hideNotification() {
    this.showNofication = false;
  }

  setShowTrashOnly(showTrashOnly: boolean) {
    this.showTrashOnly = showTrashOnly;
    this.loadInventory();
  }
}

interface Inventory {
  name: string;
  category: string;
  sellIn: number;
  quality: number;
}

import {Component, OnInit} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {DateDTO, ItemDTO} from '../integration/inventory/inventory.dtos';

@Component({
  selector: 'app-inventory-page',
  templateUrl: './inventory-page.component.html',
  styleUrls: ['./inventory-page.component.scss']
})
export class InventoryPageComponent implements OnInit {

  constructor(private http: HttpClient) {
  }

  /* -- PUBLIC -- */

  public mode = '';

  public inventoryDate: string;

  public items: ItemDTO[] = [];

  public busy = false;

  public get inAvailableMode(): boolean {
    return this.mode === 'available';
  }

  public get inDiscardedMode(): boolean {
    return this.mode === 'discarded';
  }

  public get inFindItemsMode(): boolean {
    return this.mode === 'findItems';
  }

  public ngOnInit(): void {
    this.updateInventoryDate();
    this.showAvailable();
  }

  public showAvailable(): void {
    if (this.mode !== 'available') {
      this.updateAvailable();
    }
  }

  public showDiscarded(): void {
    if (this.mode !== 'discarded') {
      this.updateDiscarded();
    }
  }

  public showFindItems(): void {
    if (this.mode !== 'findItems') {
      this.mode = 'findItems';
      this.items = [];
    }
  }

  public progressInventoryDate(): void {
    this.http.post('/api/inventory/progress-date', {}).subscribe(result => {
      this.updateInventoryDate();

      if (this.inAvailableMode) {
        this.updateAvailable();
      } else if (this.inDiscardedMode) {
        this.updateDiscarded();
      }
    });
  }

  /* -- PRIVATE -- */

  private updateInventoryDate(): void {
    this.http.get('/api/inventory/inventory-date').subscribe(result => this.inventoryDate = (result as DateDTO).date);
  }

  private updateAvailable(): void {
    this.busy = true;
    this.mode = 'available';
    this.items = [];

    this.http.get('/api/inventory/available-items').subscribe(result => {
      this.items = result as ItemDTO[];
      this.busy = false;
    });
  }

  private updateDiscarded(): void {
    this.busy = true;
    this.mode = 'discarded';
    this.items = [];

    this.http.get('/api/inventory/discarded-items').subscribe(result => {
      this.items = result as ItemDTO[];
      this.busy = false;
    });
  }
}

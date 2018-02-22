import { Component, OnInit } from '@angular/core';

import { Item } from '../item';
import { ItemDataProvider } from '../item-data-provider.service';

@Component({
  selector: 'app-item-list-page',
  templateUrl: './item-list-page.component.html',
  styleUrls: ['./item-list-page.component.scss']
})
export class ItemListPageComponent implements OnInit {
  items: Item[];
  filter: string = 'all';
  isLoading: boolean;

  constructor(private itemDataProvider: ItemDataProvider) { }

  ngOnInit() {
    this.getItems();
  }

  getItems(): void {
    //get list of all items OR filter to show the "trash"
    this.isLoading = true;
    let params = {};

    if(this.filter === 'trash') {
      params['quality'] = 0;
    }

    this.itemDataProvider.list(params)
      .subscribe(items => {
        this.items = items;
        this.isLoading = false;
      });
  }

  filterChanged(event) {
    // all/trash filter changed, reload proper items
    this.filter = event.value;
    this.getItems();
  }

  endDay(): void {
    //mmake a POST to api to end the day. Then, refresh the items list.
    this.isLoading = true;

    this.itemDataProvider.endDay()
      .subscribe(response => {
        if(!!response) {
          this.getItems();
        }
      });
  }
}

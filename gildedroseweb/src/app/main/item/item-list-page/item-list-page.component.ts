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

  constructor(private itemDataProvider: ItemDataProvider) { }

  ngOnInit() {
    this.getItems();
  }

  getItems(): void {
    let params = {};

    if(this.filter === 'trash') {
      params['quality'] = 0;
    }

    this.itemDataProvider.list(params)
      .subscribe(items => this.items =items);
  }

  filterChanged(event) {
    this.filter = event.value;
    this.getItems();
  }
}

import { Component, OnInit } from '@angular/core';

import { Item } from '../item';
import { ItemDataProvider } from '../item-data-provider.service';

@Component({
  selector: 'app-item-list-page',
  templateUrl: './item-list-page.component.html',
  styleUrls: ['./item-list-page.component.scss']
})
export class ItemListPageComponent implements OnInit {
  item: Item[];

  constructor(private itemDataProvider: ItemDataProvider) { }

  ngOnInit() {
    this.getItems();
  }

  getItems(): void {
    this.itemDataProvider.list()
      .subscribe(items => this.items =items);
  }
}

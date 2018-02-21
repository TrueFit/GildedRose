import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';

import { Item } from '../item';
import { ItemDataProvider } from '../item-data-provider.service';

@Component({
  selector: 'app-item-detail',
  templateUrl: './item-detail.component.html',
  styleUrls: ['./item-detail.component.scss']
})
export class ItemDetailComponent implements OnInit {
  item: Item;

  constructor(
    private itemDataProvider: ItemDataProvider,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.route.paramMap
        .switchMap((params: ParamMap) => this.itemDataProvider.retrieve(+params.get('id')))
        .subscribe(response => {
            this.item = response;
        });
  }
}

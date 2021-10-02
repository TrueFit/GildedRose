import { Injectable } from '@angular/core';
import { EntityState, EntityStore, StoreConfig } from '@datorama/akita';
import { dataStores } from 'src/dataStores';
import { Item } from './inventory.model';

export interface InventoryState extends EntityState<Item, string> {}

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: dataStores.inventory, idKey: 'id' })
export class InventoryStore extends EntityStore<InventoryState> {
  constructor() {
    super();
  }
}

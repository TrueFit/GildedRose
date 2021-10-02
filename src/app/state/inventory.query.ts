import { Injectable } from '@angular/core';
import { QueryEntity } from '@datorama/akita';
import { map, skipWhile } from 'rxjs/operators';
import { InventoryState, InventoryStore } from './inventory.store';

@Injectable({ providedIn: 'root' })
export class InventoryQuery extends QueryEntity<InventoryState> {
  all$ = this.selectAll().pipe(skipWhile((a) => !a));
  trash$ = this.selectAll().pipe(
    skipWhile((a) => !a),
    map((a) => a.filter((item) => item.quality < 1))
  );
  constructor(protected store: InventoryStore) {
    super(store);
  }
}

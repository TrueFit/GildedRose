import { Injectable } from '@angular/core';
import { transaction } from '@datorama/akita';
import {
  NgEntityService,
  NgEntityServiceConfig,
} from '@datorama/akita-ng-entity-service';
import { tap } from 'rxjs/operators';
import { API } from 'src/api';
import * as uuid from 'uuid';
import {
  AgedBrie,
  BackStagePasses,
  Conjured,
  Item,
  Sulfuras,
} from './inventory.model';
import { InventoryQuery } from './inventory.query';
import { InventoryState, InventoryStore } from './inventory.store';

@Injectable({ providedIn: 'root' })
@NgEntityServiceConfig({})
export class InventoryService extends NgEntityService<InventoryState> {
  constructor(protected store: InventoryStore, private query: InventoryQuery) {
    super(store);
  }

  fetchInventory(force = false): void {
    const has = this.query.hasEntity();
    if (has && !force) {
      return;
    }

    this.getHttp()
      .get(API.inventoryURL, { responseType: 'text' })
      .pipe(
        tap((res) => {
          const exp = new RegExp(/\n/g);
          const resModified = res.replace(exp, ';');
          const lines = resModified.split(';');
          const items = lines
            .filter((line) => line.length > 0)
            .map((line) => this.processInventoryFileLine(line));
          this.store.set(items);
        })
      )
      .subscribe();
  }

  processInventoryFileLine(inventoryFileLine: string): Item {
    const values = inventoryFileLine.split(',');
    if (values.length !== 4) {
      console.error('Unexpected inventory line value: ', inventoryFileLine);
    }
    const name = values[0];
    const category = values[1];
    const daysLeft = parseInt(values[2]);
    const quality = parseInt(values[3]);
    const id = uuid.v4();
    return {
      id,
      name,
      category,
      daysLeft,
      quality,
    };
  }

  processDay(item: Item): Partial<Item> {
    const category = item.category;
    const name = item.name;
    if (category === Sulfuras.category) {
      return item;
    }
    if (name === AgedBrie._name) {
      return this.processAgedBrieDay(item);
    }

    if (category === BackStagePasses.category) {
      return this.processBackStagePassesDay(item);
    }

    if (category === Conjured.category) {
      return this.processConjuredDay(item);
    }
    return this.processBaseItemDay(item);
  }

  processBaseItemDay(item: Item): Partial<Item> {
    let daysLeft = item.daysLeft;
    let quality = item.quality;
    daysLeft--;
    if (daysLeft < 0) {
      quality -= 2;
    } else {
      quality--;
    }
    if (quality < 0) {
      quality = 0;
    }
    if (quality > 50) {
      quality = 50;
    }
    return {
      name: item.name,
      category: item.category,
      daysLeft,
      quality,
    };
  }
  processAgedBrieDay(item: Item): Partial<Item> {
    let daysLeft = item.daysLeft;
    let quality = item.quality;
    daysLeft--;
    quality++;
    if (quality < 0) {
      quality = 0;
    }
    if (quality > 50) {
      quality = 50;
    }
    return {
      name: item.name,
      category: item.category,
      daysLeft,
      quality,
    };
  }
  processBackStagePassesDay(item: Item): Partial<Item> {
    let daysLeft = item.daysLeft;
    let quality = item.quality;
    daysLeft--;
    if (daysLeft < 1) {
      quality = 0;
    } else if (daysLeft <= 5) {
      quality += 3;
    } else if (daysLeft <= 10) {
      quality += 2;
    } else {
      quality--;
    }
    if (quality < 0) {
      quality = 0;
    }
    if (quality > 50) {
      quality = 50;
    }
    return {
      name: item.name,
      category: item.category,
      daysLeft,
      quality,
    };
  }
  processConjuredDay(item: Item): Partial<Item> {
    let daysLeft = item.daysLeft;
    let quality = item.quality;
    daysLeft--;
    quality -= 2;
    if (quality < 0) {
      quality = 0;
    }
    if (quality > 50) {
      quality = 50;
    }
    return {
      name: item.name,
      category: item.category,
      daysLeft,
      quality,
    };
  }

  updateItem(item: Item): void {
    const updatedItem = this.processDay(item);
    this.store.update(item.id, updatedItem);
  }

  @transaction()
  processDaysOnEntireInventory(numberOfDaysToProcess: number): void {
    Array.of([numberOfDaysToProcess]).forEach((day) => {
      this.query.getAll().forEach((item) => this.updateItem(item));
    });
  }
}

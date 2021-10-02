import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatTable } from '@angular/material/table';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { shareReplay, tap } from 'rxjs/operators';
import { InventoryService } from 'src/app/state/inventory.service';
import { Item } from '../../state/inventory.model';
import { InventoryQuery } from '../../state/inventory.query';

@UntilDestroy()
@Component({
  templateUrl: './trash.component.html',
  styleUrls: ['./trash.component.scss'],
})
export class TrashComponent implements AfterViewInit {
  @ViewChild(MatTable) table!: MatTable<Item>;
  displayedColumns = ['name', 'category', 'daysLeft', 'quality'];
  constructor(
    private readonly query: InventoryQuery,
    private readonly service: InventoryService
  ) {
    this.service.fetchInventory();
    this.query.trash$.pipe(untilDestroyed(this)).subscribe();
  }

  ngAfterViewInit(): void {
    this.query.trash$
      .pipe(
        untilDestroyed(this),
        shareReplay(),
        tap((trash) => {
          this.table.dataSource = trash;
        })
      )
      .subscribe();
  }

  getDaysLeft(item: Item): string {
    if (item.daysLeft > -1) {
      return `${item.daysLeft} days`;
    }
    return 'Past Due Date!';
  }
}

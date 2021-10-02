import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatTable } from '@angular/material/table';
import { Router } from '@angular/router';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Observable } from 'rxjs';
import { map, shareReplay, startWith, switchMap, tap } from 'rxjs/operators';
import { Item } from './../../state/inventory.model';
import { InventoryQuery } from './../../state/inventory.query';
import { InventoryService } from './../../state/inventory.service';

@UntilDestroy()
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements AfterViewInit {
  @ViewChild(MatTable) table!: MatTable<Item>;
  displayedColumns = ['name', 'category', 'daysLeft', 'quality'];
  nameControl = new FormControl();
  filteredItems?: Observable<string[]>;

  constructor(
    private readonly router: Router,
    private readonly service: InventoryService,
    private readonly query: InventoryQuery
  ) {
    this.service.fetchInventory();
  }
  ngOnInit() {
    this.filteredItems = this.nameControl.valueChanges.pipe(
      startWith(''),
      switchMap((value) =>
        this.query
          .selectAll()
          .pipe(
            map((all) =>
              all
                .map((item) => item.name)
                .filter((name) =>
                  name.toLowerCase().includes(value.toLowerCase())
                )
            )
          )
      )
    );
  }
  ngAfterViewInit(): void {
    this.query.all$
      .pipe(
        untilDestroyed(this),
        shareReplay(),
        tap((all) => {
          this.table.dataSource = all;
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

  goToItem(e: MatAutocompleteSelectedEvent): void {
    if (e.option && e.option.value && e.option.selected) {
      this.router.navigateByUrl(`/detailsByName/${e.option.value}`);
    }
  }
}

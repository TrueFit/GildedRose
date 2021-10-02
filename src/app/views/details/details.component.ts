import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { RouterQuery } from '@datorama/akita-ng-router-store';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { Item } from 'src/app/state/inventory.model';
import { InventoryService } from 'src/app/state/inventory.service';
import { InventoryQuery } from './../../state/inventory.query';

@UntilDestroy()
@Component({
  selector: 'app-details',
  templateUrl: './details.component.html',
  styleUrls: ['./details.component.scss'],
})
export class DetailsComponent implements OnInit {
  public item$?: Observable<Item | undefined>;
  constructor(
    private readonly router: Router,
    private readonly routerQuery: RouterQuery,
    private readonly inventoryService: InventoryService,
    private readonly inventoryQuery: InventoryQuery
  ) {
    if (!this.inventoryQuery.hasEntity()) {
      this.router.navigateByUrl('/');
    }
  }

  ngOnInit(): void {
    this.routerQuery
      .selectParams<string>('id')
      .pipe(
        untilDestroyed(this),
        tap((id) => {
          if (!this.inventoryQuery.hasEntity()) {
            this.router.navigateByUrl('/');
            return;
          }
          if (id) {
            this.item$ = this.inventoryQuery.selectEntity(id);
          }
        })
      )
      .subscribe();

    this.routerQuery
      .selectParams<string>('name')
      .pipe(
        untilDestroyed(this),
        tap((name) => {
          if (!this.inventoryQuery.hasEntity()) {
            this.inventoryService.fetchInventory();
          }
          if (name) {
            this.item$ = this.inventoryQuery.selectEntity(
              (i) => i.name === name
            );
          }
        })
      )
      .subscribe();
  }
}

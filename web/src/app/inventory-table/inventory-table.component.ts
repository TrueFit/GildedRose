import {Component, ViewChild, AfterViewInit, Input, EventEmitter, Output} from '@angular/core';
import {InventoryItem} from "../shared/interfaces";
import {MatSort} from '@angular/material/sort';
import {Observable} from 'rxjs';
import {MatTableDataSource} from "@angular/material/table";

@Component({
  selector: 'app-inventory-table',
  templateUrl: './inventory-table.component.html',
  styleUrls: ['./inventory-table.component.css']
})
export class InventoryTableComponent implements AfterViewInit {
	dataSource: MatTableDataSource<InventoryItem> = new MatTableDataSource<InventoryItem>();
	displayedColumns: string[] = ['name', 'category', 'sellByDays', 'quality'];

	@Input() inventory: Observable<Array<InventoryItem>> = new Observable<Array<InventoryItem>>();
	@ViewChild(MatSort) sort: MatSort = new MatSort();

	ngAfterViewInit() {
		this.inventory.subscribe(
			(response) => {
				this.dataSource = new MatTableDataSource<InventoryItem>(response);
				this.dataSource.sort = this.sort;
			}
		);
	}

	@Output() trashClicked = new EventEmitter<Event>();

	public onTrash(event: Event): void {
		this.trashClicked.emit(event);
	}
}

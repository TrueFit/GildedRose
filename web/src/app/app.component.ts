import {Component, Inject, OnInit} from '@angular/core';
import {InventoryItem} from "./shared/interfaces";
import {InventoryService} from "./shared/inventory.service";
import {BehaviorSubject} from "rxjs";
import { environment } from '../environments/environment';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'gilded-rose';
  host = environment.apiUrl;
  subject: BehaviorSubject<Array<InventoryItem>> = new BehaviorSubject<Array<InventoryItem>>(new Array<InventoryItem>());

  public get inventoryList() {
	  return this.subject.asObservable();
  }

  public currentDate: number = Date.now();

  constructor(@Inject('Window') private window: Window, private inventoryService: InventoryService) {
  }

  public ngOnInit(): void {
		this.inventoryService.getInventory(this.host).subscribe(data => {
			this.subject.next(data);
		})
  }

  public printRequested(event: Event): void {
	  this.currentDate = Date.now();
	  this.window.print();
  }

  public uploadInventoryRequested(event: any): void {
	  var reader = new FileReader();
	  let fileContent: any;
	  reader.onload = () => {
		  fileContent = reader.result;
		  console.log(fileContent);
		  this.inventoryService.uploadInventory(this.host, fileContent).subscribe(data => {
			  this.subject.next(data);
		  }, error => {
			  this.window.alert("Failed to upload due to invalid payload." + '\n' + error.message);
		  });
	  }
	  reader.readAsText(event.fileContents);
  }

  public ageInventoryRequested(event: Event): void {
	  this.inventoryService.ageInventory(this.host).subscribe(data => {
		  this.subject.next(this.recordChange(this.subject.getValue(), data));
	  })
  }

  public deleteTrashRequested(event: Event): void {
	this.inventoryService.deleteTrash(this.host).subscribe(data => {
		this.subject.next(this.preserveChange(this.subject.getValue(), data));
	})
  }

  	private preserveChange(before: Array<InventoryItem>, after: Array<InventoryItem>): Array<InventoryItem> {
  		return after.map(value => {
  			let match: InventoryItem | undefined = before.find(f => f.name == value.name && f.category == value.category);
  			return match ? match : value;
		})
	}

    private recordChange(before: Array<InventoryItem>, after: Array<InventoryItem>): Array<InventoryItem> {
  		let results: Array<InventoryItem> = [];
        for (let value of before) {
        	let match: InventoryItem | undefined = after.find(f => f.name == value.name && f.category == value.category);
	       if (match) {
	           	results.push({
					name: match.name,
					category: match.category,
					quality: match.quality,
					qualityChange: match.quality- value.quality,
					sellByDays: match.sellByDays,
					daysChange: match.sellByDays - value.sellByDays
				})
			   after.splice(after.indexOf(match), 1);
	       }
        }
        return results.concat(after);
    }
}

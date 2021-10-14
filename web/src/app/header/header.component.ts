import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {MatDialog} from "@angular/material/dialog";
import {FileUploadComponent} from "../file-upload/file-upload.component";
import {FileData} from "../shared/interfaces";

@Component({
	selector: 'app-header',
	templateUrl: './header.component.html',
	styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
	fileData: FileData = {fileName: '', fileContents: null};
	constructor(public dialog: MatDialog) {
	}

	ngOnInit(): void {
	}

	@Input() date: number = Date.now();
	@Output() printClicked = new EventEmitter<Event>();
	@Output() uploadClicked = new EventEmitter<FileData>();
	@Output() ageClicked = new EventEmitter<Event>();

	public onPrint(event: Event): void {
		this.printClicked.emit(event);
	}

	public openUploadDialog(event: Event): void {
		const dialogRef = this.dialog.open(FileUploadComponent, {
			data: this.fileData
		});

		dialogRef.afterClosed().subscribe(result => {
			if (result) {
				this.fileData = result;
				this.uploadClicked.emit(this.fileData);
			}
		})
	}

	public onAgeInventory(event: Event): void {
		this.ageClicked.emit(event);
	}

}

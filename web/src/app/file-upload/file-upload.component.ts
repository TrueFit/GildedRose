import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {FileData} from "../shared/interfaces";

@Component({
	selector: 'app-file-upload',
	templateUrl: './file-upload.component.html',
	styleUrls: ['./file-upload.component.css']
})
export class FileUploadComponent implements OnInit {
	constructor(public dialogRef: MatDialogRef<FileUploadComponent>,
				@Inject(MAT_DIALOG_DATA) public data: FileData) {
	}

	ngOnInit(): void {
	}

	onFileSelected(event: any): void {
		const file: File = event.target.files[0];
		if (file) {
			this.data.fileName = file.name;
			this.data.fileContents = file;
		}
	}

	onNoClick(): void {
		this.dialogRef.close();
	}
}

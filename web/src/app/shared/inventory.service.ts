import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {InventoryItem} from "./interfaces";
import {Observable} from "rxjs";

@Injectable({
	providedIn: 'root',
})
export class InventoryService {
	constructor(private httpClient: HttpClient) {}

	public getInventory(host: string): Observable<Array<InventoryItem>> {
		return this.httpClient.get<Array<InventoryItem>>(`${host}/inventory`);
	}

	public uploadInventory(host: string, csvFile: string): Observable<Array<InventoryItem>> {
		return this.httpClient.post<Array<InventoryItem>>(`${host}/inventory/update`, csvFile, {headers: {'Content-Type':'text/plain'}});
	}

	public ageInventory(host: string): Observable<Array<InventoryItem>> {
		return this.httpClient.post<Array<InventoryItem>>(`${host}/inventory/age`, null, {headers: {'Content-Type':'text/plain'}});
	}

	public deleteTrash(host: string): Observable<Array<InventoryItem>> {
		return this.httpClient.post<Array<InventoryItem>>(`${host}/inventory/trash`, null, {headers: {'Content-Type':'text/plain'}});
	}
}

import { Quality } from './quality';

export interface Item {
    inventoryItemID: number;
    inventoryID: number;
    name: string;
    sellIn: number;
    quality: Quality;
    totalQuality: number;
}
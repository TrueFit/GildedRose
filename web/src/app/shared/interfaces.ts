export interface InventoryItem {
  name: string;
  category: string;
  sellByDays: number;
  daysChange?: number;
  quality: number;
  qualityChange?: number;
}

export interface FileData {
	fileName: string;
	fileContents: File | null;
}

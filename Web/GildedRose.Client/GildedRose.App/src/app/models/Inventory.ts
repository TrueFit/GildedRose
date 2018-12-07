export interface InventoryModel {
  identifier: string;
  name: string;
  categoryId: number;
  categoryName: string;
  initialQuality: number;
  currentQuality: number;
  maxQuality: number;
  sellIn: number;
  isLegendary: boolean;
}

export namespace InventoryModel {
  export enum Filter {
    SHOW_ALL = "all",
    SHOW_ACTIVE = "trash",
  }
}

export class Item {
  id: number;
  name: string;
  category: ItemCategory;
  sell_in: number;
  quality: number;
}

export class ItemCategory {
  id: number;
  name: string;
}

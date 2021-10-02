export interface Item {
  id: string;
  name: string;
  category: string;
  daysLeft: number;
  quality: number;
}

export class AgedBrie {
  // prepend _ to avoid collision with a reserved word
  static _name = 'Aged Brie';
}

export class Sulfuras {
  static category = 'Sulfuras';
}

export class BackStagePasses {
  static category = 'Backstage Passes';
}

export class Conjured {
  static category = 'Conjured';
}

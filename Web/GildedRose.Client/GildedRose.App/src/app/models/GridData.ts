export type Quality = {
  initial: number;
  current: number;
  max: number;
};

export type GridData = {
  id: string;
  name: string;
  categoryId: number;
  categoryName: string;
  quality: Quality;
  sellIn: number;
  isLegendary: boolean;
};

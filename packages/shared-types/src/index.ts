export enum ItemCategory {
  WEAPON = 'Weapon',
  FOOD = 'Food',
  SULFURAS = 'Sulfuras',
  BACKSTAGE_PASSES = 'Backstage Passes',
  CONJURED = 'Conjured',
  POTION = 'Potion',
  MISC = 'Misc',
  ARMOR = 'Armor',
}

export interface Item {
  id?: string;
  name: string;
  category: ItemCategory;
  sellIn: number;
  quality: number;
  createdAt?: Date;
  updatedAt?: Date;
}

export interface InventoryStats {
  totalItems: number;
  itemsByCategory: Record<ItemCategory, number>;
  averageQuality: number;
  itemsExpiringSoon: number;
  trashItems: number;
}

export interface DayProgressResult {
  updatedItems: Item[];
  stats: InventoryStats;
  message: string;
}

export interface ApiResponse<T = any> {
  success: boolean;
  data?: T;
  message?: string;
  error?: string;
}

export interface GetInventoryResponse extends ApiResponse<Item[]> {
  stats?: InventoryStats;
}

export interface GetItemResponse extends ApiResponse<Item> {}

export interface ProgressDayResponse extends ApiResponse<DayProgressResult> {}

export interface GetTrashItemsResponse extends ApiResponse<Item[]> {}

export interface CreateItemDto {
  name: string;
  category: ItemCategory;
  sellIn: number;
  quality: number;
}

export interface CreateItemResponse extends ApiResponse<Item> {}

export enum SpecialRuleType {
  AGED_BRIE = 'AGED_BRIE',
  SULFURAS = 'SULFURAS', 
  BACKSTAGE_PASSES = 'BACKSTAGE_PASSES',
  CONJURED = 'CONJURED',
  NORMAL = 'NORMAL',
}

export interface SpecialItemRule {
  id?: string;
  name: string;
  category: ItemCategory;
  ruleType: SpecialRuleType;
  description?: string;
  ruleParameters?: Record<string, any>;
  minQuality?: number;
  maxQuality?: number;
  minSellIn?: number;
  maxSellIn?: number;
  allowedQualityValues?: number[];
  customValidation?: string;
  isActive: boolean;
  createdAt?: Date;
  updatedAt?: Date;
}

export interface CreateSpecialItemRuleDto {
  name: string;
  category: ItemCategory;
  ruleType: SpecialRuleType;
  description?: string;
  ruleParameters?: Record<string, any>;
  minQuality?: number;
  maxQuality?: number;
  minSellIn?: number;
  maxSellIn?: number;
  allowedQualityValues?: number[];
  customValidation?: string;
  isActive?: boolean;
}

export interface CreateSpecialItemRuleResponse extends ApiResponse<SpecialItemRule> {}

export interface GetSpecialItemRulesResponse extends ApiResponse<SpecialItemRule[]> {}

// Quality constraints
export const QUALITY_CONSTRAINTS = {
  MIN: 0,
  MAX: 50,
  SULFURAS_QUALITY: 80,
} as const;
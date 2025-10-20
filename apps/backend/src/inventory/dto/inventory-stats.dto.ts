import { ApiProperty } from '@nestjs/swagger';
import { ItemCategory } from '@gilded-rose/shared-types';

export class InventoryStatsDto {
  @ApiProperty({ 
    description: 'Total number of items in inventory',
    example: 20
  })
  totalItems: number;

  @ApiProperty({ 
    description: 'Number of items by category',
    type: 'object',
    additionalProperties: { type: 'number' },
    example: { 
      'Weapon': 5, 
      'Food': 3, 
      'Potion': 2, 
      'Armor': 4,
      'Misc': 6
    }
  })
  itemsByCategory: Record<ItemCategory, number>;

  @ApiProperty({ 
    description: 'Average quality of all items',
    example: 25.5
  })
  averageQuality: number;

  @ApiProperty({ 
    description: 'Number of items expiring soon (sellIn <= 5)',
    example: 3
  })
  itemsExpiringSoon: number;

  @ApiProperty({ 
    description: 'Number of items with quality = 0 (trash)',
    example: 1
  })
  trashItems: number;
}
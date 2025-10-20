import { ApiProperty } from '@nestjs/swagger';
import { ItemCategory } from '@gilded-rose/shared-types';

export class ItemResponseDto {
  @ApiProperty({ 
    description: 'Unique identifier for the item',
    example: '550e8400-e29b-41d4-a716-446655440000'
  })
  id: string;

  @ApiProperty({ 
    description: 'Name of the item',
    example: 'Sword'
  })
  name: string;

  @ApiProperty({ 
    description: 'Category of the item',
    enum: ItemCategory,
    example: ItemCategory.WEAPON
  })
  category: ItemCategory;

  @ApiProperty({ 
    description: 'Number of days left to sell the item',
    example: 30
  })
  sellIn: number;

  @ApiProperty({ 
    description: 'Quality value of the item',
    minimum: 0,
    maximum: 80,
    example: 50
  })
  quality: number;

  @ApiProperty({ 
    description: 'Date when the item was created',
    type: 'string',
    format: 'date-time'
  })
  createdAt: Date;

  @ApiProperty({ 
    description: 'Date when the item was last updated',
    type: 'string',
    format: 'date-time'
  })
  updatedAt: Date;
}
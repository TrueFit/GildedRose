import { IsString, IsEnum, IsNumber, Min, Max, IsNotEmpty } from 'class-validator';
import { ApiProperty } from '@nestjs/swagger';
import { ItemCategory, QUALITY_CONSTRAINTS } from '@gilded-rose/shared-types';

export class CreateItemDto {
  @ApiProperty({
    description: 'Name of the item',
    example: 'Iron Sword',
    minLength: 1,
  })
  @IsString()
  @IsNotEmpty()
  name: string;

  @ApiProperty({
    description: 'Category of the item',
    enum: ItemCategory,
    example: ItemCategory.WEAPON,
  })
  @IsEnum(ItemCategory)
  category: ItemCategory;

  @ApiProperty({
    description: 'Number of days before item expires',
    example: 30,
    minimum: -100,
    maximum: 1000,
  })
  @IsNumber()
  @Min(-100)
  @Max(1000)
  sellIn: number;

  @ApiProperty({
    description: 'Quality of the item (0-50, except Sulfuras which is 80)',
    example: 45,
    minimum: QUALITY_CONSTRAINTS.MIN,
    maximum: QUALITY_CONSTRAINTS.SULFURAS_QUALITY,
  })
  @IsNumber()
  @Min(QUALITY_CONSTRAINTS.MIN)
  @Max(QUALITY_CONSTRAINTS.SULFURAS_QUALITY)
  quality: number;
}
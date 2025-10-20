import { ApiProperty } from '@nestjs/swagger';
import { ItemResponseDto } from './item-response.dto';
import { InventoryStatsDto } from './inventory-stats.dto';

export class GetAllItemsResponseDto {
  @ApiProperty({ 
    description: 'Indicates if the request was successful',
    example: true
  })
  success: boolean;

  @ApiProperty({ 
    description: 'Array of inventory items',
    type: [ItemResponseDto]
  })
  data: ItemResponseDto[];

  @ApiProperty({ 
    description: 'Inventory statistics',
    type: InventoryStatsDto,
    required: false
  })
  stats?: InventoryStatsDto;

  @ApiProperty({ 
    description: 'Response message',
    example: 'Retrieved 20 items from inventory'
  })
  message: string;
}
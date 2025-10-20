import { ApiProperty } from '@nestjs/swagger';
import { ItemResponseDto } from './item-response.dto';

export class GetItemByNameResponseDto {
  @ApiProperty({ 
    description: 'Indicates if the request was successful',
    example: true
  })
  success: boolean;

  @ApiProperty({ 
    description: 'The requested item',
    type: ItemResponseDto
  })
  data: ItemResponseDto;

  @ApiProperty({ 
    description: 'Response message',
    example: 'Found item: Sword'
  })
  message: string;
}
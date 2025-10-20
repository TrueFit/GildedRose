import { ApiProperty } from '@nestjs/swagger';

export class ErrorResponseDto {
  @ApiProperty({ 
    description: 'Indicates if the request was successful',
    example: false
  })
  success: boolean;

  @ApiProperty({ 
    description: 'Error type',
    example: 'Item not found'
  })
  error: string;

  @ApiProperty({ 
    description: 'Detailed error message',
    example: 'Item with name \'NonExistentItem\' not found'
  })
  message: string;
}
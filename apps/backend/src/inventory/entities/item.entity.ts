import { Entity, PrimaryGeneratedColumn, Column, CreateDateColumn, UpdateDateColumn } from 'typeorm';
import { ItemCategory } from '@gilded-rose/shared-types';
import { ApiProperty } from '@nestjs/swagger';

@Entity('items')
export class ItemEntity {
  @ApiProperty({
    description: 'Unique identifier for the item',
    example: 'a1b2c3d4-e5f6-7890-ab12-cd34ef56gh78',
  })
  @PrimaryGeneratedColumn('uuid')
  id: string;

  @ApiProperty({ 
    description: 'Name of the item',
    example: 'Iron Sword',
  })
  @Column({ type: 'varchar', length: 255 })
  name: string;

  @ApiProperty({
    description: 'Category of the item',
    example: 'weapon',
  })
  @Column({
    type: 'varchar',
    length: 50,
    enum: ItemCategory 
  })
  category: ItemCategory;

  @ApiProperty({
    description: 'SellIn value of the item',
    example: 30,
  })
  @Column({ type: 'integer' })
  sellIn: number;

  @ApiProperty({
    description: 'Quality value of the item',
    example: 50,
  })
  @Column({ type: 'integer' })
  quality: number;

  @ApiProperty({
    description: 'Timestamp when the item was created',
    example: '2024-06-01T12:00:00Z',
  })
  @CreateDateColumn()
  createdAt: Date;

  @ApiProperty({
    description: 'Timestamp when the item was last updated',
    example: '2024-06-01T12:00:00Z',
  })
  @UpdateDateColumn()
  updatedAt: Date;
}
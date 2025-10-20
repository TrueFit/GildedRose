import { Entity, PrimaryGeneratedColumn, Column, CreateDateColumn, UpdateDateColumn } from 'typeorm';
import { ItemCategory } from '@gilded-rose/shared-types';

export enum SpecialRuleType {
  AGED_BRIE = 'AGED_BRIE',
  SULFURAS = 'SULFURAS', 
  BACKSTAGE_PASSES = 'BACKSTAGE_PASSES',
  CONJURED = 'CONJURED',
  NORMAL = 'NORMAL', // For regular items without special behavior
}

@Entity('special_item_rules')
export class SpecialItemRuleEntity {
  @PrimaryGeneratedColumn('uuid')
  id: string;

  @Column({ type: 'varchar', length: 255 })
  name: string;

  @Column({ 
    type: 'varchar', 
    length: 50,
    enum: ItemCategory 
  })
  category: ItemCategory;

  @Column({ 
    type: 'varchar', 
    length: 50,
    enum: SpecialRuleType 
  })
  ruleType: SpecialRuleType;

  @Column({ type: 'text', nullable: true })
  description: string;

  @Column({ type: 'json', nullable: true })
  ruleParameters: Record<string, any>;

  // Validation constraints
  @Column({ type: 'integer', nullable: true })
  minQuality?: number;

  @Column({ type: 'integer', nullable: true })
  maxQuality?: number;

  @Column({ type: 'integer', nullable: true })
  minSellIn?: number;

  @Column({ type: 'integer', nullable: true })
  maxSellIn?: number;

  @Column({ type: 'json', nullable: true })
  allowedQualityValues?: number[];

  @Column({ type: 'text', nullable: true })
  customValidation?: string;

  @Column({ type: 'boolean', default: true })
  isActive: boolean;

  @CreateDateColumn()
  createdAt: Date;

  @UpdateDateColumn()
  updatedAt: Date;
}
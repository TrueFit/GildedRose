import { IsString, IsEnum, IsOptional, IsBoolean, IsNumber, IsArray, IsObject } from 'class-validator';
import { ItemCategory, SpecialRuleType } from '@gilded-rose/shared-types';

export class CreateSpecialItemRuleDto {
  @IsString()
  name: string;

  @IsEnum(ItemCategory)
  category: ItemCategory;

  @IsEnum(SpecialRuleType)
  ruleType: SpecialRuleType;

  @IsOptional()
  @IsString()
  description?: string;

  @IsOptional()
  @IsObject()
  ruleParameters?: Record<string, any>;

  @IsOptional()
  @IsNumber()
  minQuality?: number;

  @IsOptional()
  @IsNumber()
  maxQuality?: number;

  @IsOptional()
  @IsNumber()
  minSellIn?: number;

  @IsOptional()
  @IsNumber()
  maxSellIn?: number;

  @IsOptional()
  @IsArray()
  @IsNumber({}, { each: true })
  allowedQualityValues?: number[];

  @IsOptional()
  @IsString()
  customValidation?: string;

  @IsOptional()
  @IsBoolean()
  isActive?: boolean;
}
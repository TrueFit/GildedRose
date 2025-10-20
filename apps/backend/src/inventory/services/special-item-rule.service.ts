import { Injectable, NotFoundException, ConflictException, BadRequestException } from '@nestjs/common';
import { InjectRepository } from '@nestjs/typeorm';
import { Repository } from 'typeorm';
import { SpecialItemRule, CreateItemDto, SpecialRuleType } from '@gilded-rose/shared-types';
import { SpecialItemRuleEntity } from '../entities/special-item-rule.entity';
import { CreateSpecialItemRuleDto } from '../dto/create-special-item-rule.dto';

@Injectable()
export class SpecialItemRuleService {
  constructor(
    @InjectRepository(SpecialItemRuleEntity)
    private specialRuleRepository: Repository<SpecialItemRuleEntity>,
  ) {}

  /**
   * Create a new special item rule or update existing one
   */
  async createOrUpdateSpecialItemRule(createRuleDto: CreateSpecialItemRuleDto): Promise<SpecialItemRule> {
    // Check if rule already exists for this item name
    const existingRule = await this.specialRuleRepository.findOne({
      where: { name: createRuleDto.name }
    });

    if (existingRule) {
      // Update existing rule
      Object.assign(existingRule, createRuleDto);
      existingRule.updatedAt = new Date();
      const updatedRule = await this.specialRuleRepository.save(existingRule);
      return this.entityToSpecialItemRule(updatedRule);
    } else {
      // Create new rule
      const newRule = this.specialRuleRepository.create({
        ...createRuleDto,
        isActive: createRuleDto.isActive ?? true,
      });
      const savedRule = await this.specialRuleRepository.save(newRule);
      return this.entityToSpecialItemRule(savedRule);
    }
  }

  /**
   * Get all special item rules
   */
  async getAllSpecialItemRules(): Promise<SpecialItemRule[]> {
    const rules = await this.specialRuleRepository.find();
    return rules.map(rule => this.entityToSpecialItemRule(rule));
  }

  /**
   * Get special item rule by name
   */
  async getSpecialItemRuleByName(name: string): Promise<SpecialItemRule> {
    const rule = await this.specialRuleRepository.findOne({
      where: { name, isActive: true }
    });

    if (!rule) {
      throw new NotFoundException(`Special item rule for '${name}' not found`);
    }

    return this.entityToSpecialItemRule(rule);
  }

  /**
   * Get special item rules by category
   */
  async getSpecialItemRulesByCategory(category: string): Promise<SpecialItemRule[]> {
    const rules = await this.specialRuleRepository.find({
      where: { category: category as any, isActive: true }
    });

    return rules.map(rule => this.entityToSpecialItemRule(rule));
  }

  /**
   * Delete a special item rule
   */
  async deleteSpecialItemRule(id: string): Promise<void> {
    const rule = await this.specialRuleRepository.findOne({ where: { id } });
    
    if (!rule) {
      throw new NotFoundException(`Special item rule with ID '${id}' not found`);
    }

    await this.specialRuleRepository.remove(rule);
  }

  /**
   * Validate item creation based on special item rules
   */
  async validateItem(createItemDto: CreateItemDto): Promise<{ isValid: boolean; errors: string[] }> {
    const errors: string[] = [];

    try {
      // Try to find a specific rule for this item name first
      let rule = await this.specialRuleRepository.findOne({
        where: { name: createItemDto.name, isActive: true }
      });

      // If no specific rule, try to find a category-based rule
      if (!rule) {
        rule = await this.specialRuleRepository.findOne({
          where: { category: createItemDto.category, isActive: true }
        });
      }

      if (rule) {
        // Validate against the rule
        if (rule.minQuality !== null && rule.minQuality !== undefined && createItemDto.quality < rule.minQuality) {
          errors.push(`Quality must be at least ${rule.minQuality} for ${createItemDto.name}`);
        }

        if (rule.maxQuality !== null && rule.maxQuality !== undefined && createItemDto.quality > rule.maxQuality) {
          errors.push(`Quality must not exceed ${rule.maxQuality} for ${createItemDto.name}`);
        }

        if (rule.minSellIn !== null && rule.minSellIn !== undefined && createItemDto.sellIn < rule.minSellIn) {
          errors.push(`SellIn must be at least ${rule.minSellIn} for ${createItemDto.name}`);
        }

        if (rule.maxSellIn !== null && rule.maxSellIn !== undefined && createItemDto.sellIn > rule.maxSellIn) {
          errors.push(`SellIn must not exceed ${rule.maxSellIn} for ${createItemDto.name}`);
        }

        if (rule.allowedQualityValues && rule.allowedQualityValues.length > 0) {
          if (!rule.allowedQualityValues.includes(createItemDto.quality)) {
            errors.push(`Quality must be one of: ${rule.allowedQualityValues.join(', ')} for ${createItemDto.name}`);
          }
        }

        // Special validations for specific rule types
        if (rule.ruleType === SpecialRuleType.SULFURAS) {
          if (createItemDto.quality !== 80) {
            errors.push('Sulfuras items must have quality of 80');
          }
        }
      }

      // General validation rules that apply to all items
      if (createItemDto.quality < 0) {
        errors.push('Quality cannot be negative');
      }

      if (createItemDto.quality > 80) {
        errors.push('Quality cannot exceed 80');
      }

      if (createItemDto.sellIn < -100) {
        errors.push('SellIn cannot be less than -100');
      }

      if (createItemDto.sellIn > 1000) {
        errors.push('SellIn cannot exceed 1000');
      }

    } catch (error) {
      console.error('Error validating item:', error);
      errors.push('Validation error occurred');
    }

    return {
      isValid: errors.length === 0,
      errors
    };
  }

  /**
   * Convert entity to interface
   */
  private entityToSpecialItemRule = (entity: SpecialItemRuleEntity): SpecialItemRule => ({
    id: entity.id,
    name: entity.name,
    category: entity.category,
    ruleType: entity.ruleType,
    description: entity.description,
    ruleParameters: entity.ruleParameters,
    minQuality: entity.minQuality,
    maxQuality: entity.maxQuality,
    minSellIn: entity.minSellIn,
    maxSellIn: entity.maxSellIn,
    allowedQualityValues: entity.allowedQualityValues,
    customValidation: entity.customValidation,
    isActive: entity.isActive,
    createdAt: entity.createdAt,
    updatedAt: entity.updatedAt,
  });
}
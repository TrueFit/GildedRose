import { Injectable } from '@nestjs/common';
import { InjectRepository } from '@nestjs/typeorm';
import { Repository } from 'typeorm';
import { Item, ItemCategory, QUALITY_CONSTRAINTS } from '@gilded-rose/shared-types';
import { SpecialItemRuleEntity, SpecialRuleType } from '../entities/special-item-rule.entity';

@Injectable()
export class QualityUpdateService {
  constructor(
    @InjectRepository(SpecialItemRuleEntity)
    private specialRuleRepository: Repository<SpecialItemRuleEntity>,
  ) {}

  /**
   * Updates all items based on business rules loaded from database
   */
  async updateItems(items: Item[]): Promise<Item[]> {
    const specialRules = await this.specialRuleRepository.find({ 
      where: { isActive: true } 
    });
    
    return items.map(item => this.updateSingleItem({ ...item }, specialRules));
  }

  /**
   * Updates a single item based on business rules loaded from database
   */
  private updateSingleItem(item: Item, specialRules: SpecialItemRuleEntity[]): Item {
    // Sulfuras never changes
    if (item.category === ItemCategory.SULFURAS) {
      return item;
    }

    // Decrease sellIn for all items except Sulfuras
    item.sellIn -= 1;

    // Update quality based on item type and special rules
    item.quality = this.calculateNewQuality(item, specialRules);

    // Ensure quality stays within bounds (except Sulfuras)
    item.quality = Math.max(QUALITY_CONSTRAINTS.MIN, 
                           Math.min(QUALITY_CONSTRAINTS.MAX, item.quality));

    item.updatedAt = new Date();
    
    return item;
  }

  private calculateNewQuality(item: Item, specialRules: SpecialItemRuleEntity[]): number {
    let newQuality = item.quality;

    // Check if item has special rules
    const specialRule = specialRules.find(rule => 
      rule.name === item.name || 
      (rule.category === item.category && this.matchesSpecialRule(item, rule))
    );

    if (specialRule) {
      return this.applySpecialRule(item, specialRule);
    }

    // Default rules for regular items
    switch (item.category) {
      case ItemCategory.CONJURED:
        // Conjured items degrade twice as fast
        newQuality -= item.sellIn < 0 ? 4 : 2;
        break;

      default:
        // Regular items (weapons, armor, potions, misc)
        newQuality -= item.sellIn < 0 ? 2 : 1;
        break;
    }

    return newQuality;
  }

  private matchesSpecialRule(item: Item, rule: SpecialItemRuleEntity): boolean {
    switch (rule.ruleType) {
      case SpecialRuleType.AGED_BRIE:
        return item.name === 'Aged Brie';
      case SpecialRuleType.SULFURAS:
        return item.name.includes('Ragnaros') || item.category === ItemCategory.SULFURAS;
      case SpecialRuleType.BACKSTAGE_PASSES:
        return item.category === ItemCategory.BACKSTAGE_PASSES;
      default:
        return false;
    }
  }

  private applySpecialRule(item: Item, rule: SpecialItemRuleEntity): number {
    const params = rule.ruleParameters;
    let newQuality = item.quality;

    switch (rule.ruleType) {
      case SpecialRuleType.AGED_BRIE:
        newQuality += item.sellIn < 0 ? 
          (params.qualityIncreaseAfterSellDate || 2) : 
          (params.qualityIncrease || 1);
        break;

      case SpecialRuleType.SULFURAS:
        return params.quality || 80;

      case SpecialRuleType.BACKSTAGE_PASSES:
        if (item.sellIn < 0) {
          // After concert, quality drops to 0
          newQuality = params.qualityAfterConcert || 0;
        } else if (item.sellIn < 5) {
          // Less than 5 days: increase by 3
          newQuality += params.qualityIncrease5Days || 3;
        } else if (item.sellIn < 10) {
          // Less than 10 days: increase by 2
          newQuality += params.qualityIncrease10Days || 2;
        } else {
          // More than 10 days: increase by 1
          newQuality += params.qualityIncreaseDefault || 1;
        }
        break;
      
      // Conjured items degrade twice as fast
      case SpecialRuleType.CONJURED:
        newQuality -= item.sellIn < 0 ? 4 : 2;
        break;

      default:
        // Fallback to default behavior
        newQuality -= item.sellIn < 0 ? 2 : 1;
        break;
    }

    return newQuality;
  }
}
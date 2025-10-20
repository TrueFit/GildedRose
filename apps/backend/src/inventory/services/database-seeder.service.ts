import { Injectable, Logger } from '@nestjs/common';
import { InjectRepository } from '@nestjs/typeorm';
import { Repository } from 'typeorm';
import { ItemEntity } from '../entities/item.entity';
import { SpecialItemRuleEntity, SpecialRuleType } from '../entities/special-item-rule.entity';

import { ItemCategory } from '@gilded-rose/shared-types';
import * as fs from 'fs';
import * as path from 'path';

@Injectable()
export class DatabaseSeederService {
  private readonly logger = new Logger(DatabaseSeederService.name);

  constructor(
    @InjectRepository(ItemEntity)
    private itemRepository: Repository<ItemEntity>,
    @InjectRepository(SpecialItemRuleEntity)
    private specialRuleRepository: Repository<SpecialItemRuleEntity>,
  ) {}

  async seedDatabase(): Promise<void> {
    try {
      await this.seedSpecialRules();
      await this.seedInitialInventory();
      this.logger.log('Database seeding completed successfully');
    } catch (error) {
      this.logger.error('Database seeding failed', error);
      throw error;
    }
  }

  private async seedSpecialRules(): Promise<void> {
    const existingRules = await this.specialRuleRepository.count();
    if (existingRules > 0) {
      this.logger.log('Special rules already exist, skipping seed');
      return;
    }

    // Initial special rules defined in bussines logic
    const specialRules = [
      {
        name: 'Aged Brie',
        category: ItemCategory.FOOD,
        ruleType: SpecialRuleType.AGED_BRIE,
        description: 'Increases in quality the older it gets',
        ruleParameters: {
          qualityIncrease: 1,
          qualityIncreaseAfterSellDate: 2,
          maxQuality: 50,
        },
        minQuality: 0,
        maxQuality: 50,
        minSellIn: -100,
        maxSellIn: 1000,
      },
      {
        name: 'Hand of Ragnaros',
        category: ItemCategory.SULFURAS,
        ruleType: SpecialRuleType.SULFURAS,
        description: 'Legendary item that never has to be sold or decreases in quality',
        ruleParameters: {
          quality: 80,
          neverChanges: true,
        },
        allowedQualityValues: [80],
        minSellIn: 0,
        maxSellIn: 0,
      },
      {
        name: 'I am Murloc',
        category: ItemCategory.BACKSTAGE_PASSES,
        ruleType: SpecialRuleType.BACKSTAGE_PASSES,
        description: 'Increases in quality as SellIn value approaches',
        ruleParameters: {
          qualityIncreaseDefault: 1,
          qualityIncrease10Days: 2,
          qualityIncrease5Days: 3,
          qualityAfterConcert: 0,
        },
      },
      {
        name: 'Raging Ogre',
        category: ItemCategory.BACKSTAGE_PASSES,
        ruleType: SpecialRuleType.BACKSTAGE_PASSES,
        description: 'Increases in quality as SellIn value approaches',
        ruleParameters: {
          qualityIncreaseDefault: 1,
          qualityIncrease10Days: 2,
          qualityIncrease5Days: 3,
          qualityAfterConcert: 0,
        },
      },
      {
        name: 'TAFKAL80ETC Concert',
        category: ItemCategory.BACKSTAGE_PASSES,
        ruleType: SpecialRuleType.BACKSTAGE_PASSES,
        description: 'Increases in quality as SellIn value approaches',
        ruleParameters: {
          qualityIncreaseDefault: 1,
          qualityIncrease10Days: 2,
          qualityIncrease5Days: 3,
          qualityAfterConcert: 0,
        },
      },
    ];

    for (const ruleData of specialRules) {
      const rule = this.specialRuleRepository.create(ruleData);
      await this.specialRuleRepository.save(rule);
    }

    this.logger.log(`Seeded ${specialRules.length} special rules`);
  }

  private async seedInitialInventory(): Promise<void> {
    const existingItems = await this.itemRepository.count();
    if (existingItems > 0) {
      this.logger.log('Inventory already exists, skipping seed');
      return;
    }

    try {
      const inventoryFilePath = path.join(process.cwd(), '../../inventory.txt');
      const inventoryData = fs.readFileSync(inventoryFilePath, 'utf-8');
      const lines = inventoryData.trim().split('\n');
      
      const items = lines.map(line => {
        const [name, category, sellIn, quality] = line.split(',');
        return this.itemRepository.create({
          name: name.trim(),
          category: category.trim() as ItemCategory,
          sellIn: parseInt(sellIn.trim()),
          quality: parseInt(quality.trim()),
        });
      });

      await this.itemRepository.save(items);
      this.logger.log(`Seeded ${items.length} items from inventory.txt`);
    } catch (error) {
      this.logger.error('Failed to load initial inventory from inventory.txt', error);
    }
  }

  async clearDatabase(): Promise<void> {
    await this.itemRepository.clear();
    await this.specialRuleRepository.clear();
    this.logger.log('Database cleared');
  }

  async getDatabaseStatus(): Promise<{
    itemsCount: number;
    specialRulesCount: number;
    isSeeded: boolean;
  }> {
    const itemsCount = await this.itemRepository.count();
    const specialRulesCount = await this.specialRuleRepository.count();
    
    return {
      itemsCount,
      specialRulesCount,
      isSeeded: itemsCount > 0 && specialRulesCount > 0,
    };
  }

  async checkSeedingRequired(): Promise<{
    needsItemSeeding: boolean;
    needsRuleSeeding: boolean;
  }> {
    const itemsCount = await this.itemRepository.count();
    const specialRulesCount = await this.specialRuleRepository.count();
    
    return {
      needsItemSeeding: itemsCount === 0,
      needsRuleSeeding: specialRulesCount === 0,
    };
  }

  async reseedDatabase(): Promise<void> {
    this.logger.log('Reseeding database - clearing existing data first');
    await this.clearDatabase();
    await this.seedDatabase();
  }


}
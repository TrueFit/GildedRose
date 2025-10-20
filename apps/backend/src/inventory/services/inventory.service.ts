import { Injectable, NotFoundException, OnModuleInit } from '@nestjs/common';
import { InjectRepository } from '@nestjs/typeorm';
import { Repository } from 'typeorm';
import { Item, ItemCategory, InventoryStats, DayProgressResult } from '@gilded-rose/shared-types';
import { QualityUpdateService } from './quality-update.service';
import { DatabaseSeederService } from './database-seeder.service';
import { ItemEntity } from '../entities/item.entity';

@Injectable()
export class InventoryService implements OnModuleInit {
  constructor(
    @InjectRepository(ItemEntity)
    private itemRepository: Repository<ItemEntity>,
    private readonly qualityUpdateService: QualityUpdateService,
    private readonly databaseSeederService: DatabaseSeederService,
  ) {}

  async onModuleInit() {
    await this.databaseSeederService.seedDatabase();
  }

  /**
   * Get all items in inventory
   */
  async getAllItems(): Promise<Item[]> {
    const items = await this.itemRepository.find();
    return items.map(this.entityToItem);
  }

  /**
   * Get a single item by name
   */
  async getItemByName(name: string): Promise<Item> {
    const item = await this.itemRepository.findOne({ 
      where: { name: name } 
    });
    
    if (!item) {
      throw new NotFoundException(`Item '${name}' not found in inventory`);
    }
    
    return this.entityToItem(item);
  }

  /**
   * Progress to the next day - updates all item qualities
   */
  async progressToNextDay(): Promise<DayProgressResult> {
    const beforeStats = await this.calculateStats();
    
    const items = await this.itemRepository.find();
    const itemsAsInterface = items.map(this.entityToItem);
    
    // Update qualities
    const updatedItems = await this.qualityUpdateService.updateItems(itemsAsInterface);
    
    // Save back to database
    const updatedEntities = updatedItems.map(item => {
      const entity = new ItemEntity();
      entity.id = item.id!;
      entity.name = item.name;
      entity.category = item.category;
      entity.sellIn = item.sellIn;
      entity.quality = item.quality;
      entity.updatedAt = new Date();
      return entity;
    });
    
    await this.itemRepository.save(updatedEntities);
    
    const afterStats = await this.calculateStats();
    
    return {
      updatedItems,
      stats: afterStats,
      message: `Updated ${updatedItems.length} products. Quality changes: ${afterStats.averageQuality.toFixed(1)} avg (was ${beforeStats.averageQuality.toFixed(1)})`
    };
  }

  /**
   * Get all items that should be thrown away (quality = 0)
   */
  async getTrashItems(): Promise<Item[]> {
    const items = await this.itemRepository.find({ 
      where: { quality: 0 } 
    });
    return items.map(this.entityToItem);
  }

  /**
   * Calculate inventory statistics
   */
  async calculateStats(): Promise<InventoryStats> {
    const items = await this.itemRepository.find();
    
    const itemsByCategory = items.reduce((acc, item) => {
      acc[item.category] = (acc[item.category] || 0) + 1;
      return acc;
    }, {} as Record<ItemCategory, number>);

    const totalQuality = items.reduce((sum, item) => sum + item.quality, 0);
    const averageQuality = items.length > 0 ? totalQuality / items.length : 0;
    
    const itemsExpiringSoon = items.filter(item => 
      item.sellIn <= 5 && item.sellIn > 0
    ).length;
    
    const trashItems = items.filter(item => item.quality === 0).length;

    return {
      totalItems: items.length,
      itemsByCategory,
      averageQuality,
      itemsExpiringSoon,
      trashItems,
    };
  }

  /**
   * Add a new item to inventory
   */
  async addItem(itemData: Omit<Item, 'id' | 'createdAt' | 'updatedAt'>): Promise<Item> {
    const entity = this.itemRepository.create({
      name: itemData.name,
      category: itemData.category,
      sellIn: itemData.sellIn,
      quality: itemData.quality,
    });
    
    const savedEntity = await this.itemRepository.save(entity);
    return this.entityToItem(savedEntity);
  }

  /**
   * Remove an item from inventory
   */
  async removeItem(itemId: string): Promise<boolean> {
    const result = await this.itemRepository.delete(itemId);
    return result.affected! > 0;
  }

  /**
   * Convert ItemEntity to Item interface
   */
  private entityToItem(entity: ItemEntity): Item {
    return {
      id: entity.id,
      name: entity.name,
      category: entity.category,
      sellIn: entity.sellIn,
      quality: entity.quality,
      createdAt: entity.createdAt,
      updatedAt: entity.updatedAt,
    };
  }
}
import { Test, TestingModule } from '@nestjs/testing';
import { QualityUpdateService } from './quality-update.service';
import { Item, ItemCategory } from '@gilded-rose/shared-types';
import { getRepositoryToken } from '@nestjs/typeorm';
import { Repository } from 'typeorm';
import { SpecialItemRuleEntity } from '../entities/special-item-rule.entity';

describe('Quality update service', () => {
  let service: QualityUpdateService;
  let mockRepository: Partial<Repository<SpecialItemRuleEntity>>;

  beforeEach(async () => {
    mockRepository = {
      find: jest.fn().mockResolvedValue([]),
    };

    const module: TestingModule = await Test.createTestingModule({
      providers: [
        QualityUpdateService,
        {
          provide: getRepositoryToken(SpecialItemRuleEntity),
          useValue: mockRepository,
        },
      ],
    }).compile();

    service = module.get<QualityUpdateService>(QualityUpdateService);
  });

  it('should be defined', () => {
    expect(service).toBeDefined();
  });

  describe('Normal items', () => {
    it('should decrease quality by 1 before sell date', async () => {
      const item: Item = {
        name: 'Normal Item',
        category: ItemCategory.MISC,
        sellIn: 5,
        quality: 10,
      };

      const [updatedItem] = await service.updateItems([item]);
      
      expect(updatedItem.quality).toBe(9);
      expect(updatedItem.sellIn).toBe(4);
    });

    it('should decrease quality by 2 after sell date', async () => {
      const item: Item = {
        name: 'Normal Item',
        category: ItemCategory.MISC,
        sellIn: 0,
        quality: 10,
      };

      const [updatedItem] = await service.updateItems([item]);
      
      expect(updatedItem.quality).toBe(8);
      expect(updatedItem.sellIn).toBe(-1);
    });

    it('should not decrease quality below 0', async () => {
      const item: Item = {
        name: 'Regular Item',
        category: ItemCategory.MISC,
        sellIn: 5,
        quality: 0,
      };

      const [updatedItem] = await service.updateItems([item]);
      
      expect(updatedItem.quality).toBe(0);
    });
  });

  describe('Aged Brie', () => {
    it('should increase quality by 1 before sell date', async () => {
      const item: Item = {
        name: 'Aged Brie',
        category: ItemCategory.FOOD,
        sellIn: 5,
        quality: 10,
      };

      const [updatedItem] = await service.updateItems([item]);
      
      expect(updatedItem.quality).toBe(11);
      expect(updatedItem.sellIn).toBe(4);
    });

    it('should increase quality by 2 after sell date', async () => {
      const item: Item = {
        name: 'Aged Brie',
        category: ItemCategory.FOOD,
        sellIn: 0,
        quality: 10,
      };

      const [updatedItem] = await service.updateItems([item]);
      
      expect(updatedItem.quality).toBe(12);
      expect(updatedItem.sellIn).toBe(-1);
    });

    it('should not increase quality above 50', async () => {
      const item: Item = {
        name: 'Aged Brie',
        category: ItemCategory.FOOD,
        sellIn: 5,
        quality: 50,
      };

      const [updatedItem] = await service.updateItems([item]);
      
      expect(updatedItem.quality).toBe(50);
    });
  });

  describe('Sulfuras items', () => {
    it('should maintain quality of sulfuras items', async () => {
      const item: Item = {
        name: 'Sulfuras, Hand of Ragnaros',
        sellIn: 0,
        quality: 80,
        category: ItemCategory.MISC
      };

      const [updatedItem] = await service.updateItems([item]);
      
      expect(updatedItem.quality).toBe(80);
      expect(updatedItem.sellIn).toBe(0);
    });
  });

  describe('Backstage passes', () => {
    it('should increase quality by 1 when more than 10 days', async () => {
      const item: Item = {
        name: 'Backstage passes',
        category: ItemCategory.BACKSTAGE_PASSES,
        sellIn: 15,
        quality: 20,
      };

      const [updatedItem] = await service.updateItems([item]);
      
      expect(updatedItem.quality).toBe(21);
      expect(updatedItem.sellIn).toBe(14);
    });

    it('should increase quality by 2 when 6-10 days', async () => {
      const item: Item = {
        name: 'Backstage passes',
        category: ItemCategory.BACKSTAGE_PASSES,
        sellIn: 10,
        quality: 20,
      };

      const [updatedItem] = await service.updateItems([item]);
      
      expect(updatedItem.quality).toBe(22);
      expect(updatedItem.sellIn).toBe(9);
    });

    it('should increase quality by 3 when 1-5 days', async () => {
      const item: Item = {
        name: 'Backstage passes',
        category: ItemCategory.BACKSTAGE_PASSES,
        sellIn: 5,
        quality: 20,
      };

      const [updatedItem] = await service.updateItems([item]);
      
      expect(updatedItem.quality).toBe(23);
      expect(updatedItem.sellIn).toBe(4);
    });

    it('should drop quality to 0 after concert', async () => {
      const item: Item = {
        name: 'Backstage passes',
        category: ItemCategory.BACKSTAGE_PASSES,
        sellIn: 0,
        quality: 20,
      };

      const [updatedItem] = await service.updateItems([item]);
      
      expect(updatedItem.quality).toBe(0);
      expect(updatedItem.sellIn).toBe(-1);
    });
  });

  describe('Conjured items', () => {
    it('should decrease quality by 2 before sell date', async () => {
      const item: Item = {
        name: 'Conjured Item',
        category: ItemCategory.CONJURED,
        sellIn: 5,
        quality: 10,
      };

      const [updatedItem] = await service.updateItems([item]);
      
      expect(updatedItem.quality).toBe(8);
      expect(updatedItem.sellIn).toBe(4);
    });

    it('should decrease quality by 4 after sell date', async () => {
      const item: Item = {
        name: 'Conjured Item',
        category: ItemCategory.CONJURED,
        sellIn: 0,
        quality: 10,
      };

      const [updatedItem] = await service.updateItems([item]);
      
      expect(updatedItem.quality).toBe(6);
      expect(updatedItem.sellIn).toBe(-1);
    });
  });
});
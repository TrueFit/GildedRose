import { Module } from '@nestjs/common';
import { TypeOrmModule } from '@nestjs/typeorm';
import { InventoryController } from './inventory.controller';
import { InventoryService } from './services/inventory.service';
import { QualityUpdateService } from './services/quality-update.service';
import { DatabaseSeederService } from './services/database-seeder.service';
import { SpecialItemRuleService } from './services/special-item-rule.service';
import { ItemEntity } from './entities/item.entity';
import { SpecialItemRuleEntity } from './entities/special-item-rule.entity';

@Module({
  imports: [
    TypeOrmModule.forFeature([ItemEntity, SpecialItemRuleEntity])
  ],
  controllers: [InventoryController],
  providers: [InventoryService, QualityUpdateService, DatabaseSeederService, SpecialItemRuleService],
  exports: [InventoryService, SpecialItemRuleService],
})
export class InventoryModule {}
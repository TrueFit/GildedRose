import { Module } from '@nestjs/common';
import { TypeOrmModule } from '@nestjs/typeorm';
import { AppController } from './app.controller';
import { AppService } from './app.service';
import { InventoryModule } from './inventory/inventory.module';
import { ItemEntity } from './inventory/entities/item.entity';
import { SpecialItemRuleEntity } from './inventory/entities/special-item-rule.entity';

@Module({
  imports: [
    TypeOrmModule.forRoot({
      type: 'sqlite',
      database: 'inventory.db',
      entities: [ItemEntity, SpecialItemRuleEntity],
      synchronize: process.env.NODE_ENV !== 'production',
      logging: false,
    }),
    InventoryModule
  ],
  controllers: [AppController],
  providers: [AppService],
})
export class AppModule {}
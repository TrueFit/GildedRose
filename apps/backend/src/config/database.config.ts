import { TypeOrmModuleOptions } from '@nestjs/typeorm';
import { ItemEntity } from '../inventory/entities/item.entity';
import { SpecialItemRuleEntity } from '../inventory/entities/special-item-rule.entity';

export const databaseConfig: TypeOrmModuleOptions = {
  type: 'sqlite',
  database: 'gilded-rose.sqlite',
  entities: [ItemEntity, SpecialItemRuleEntity],
  synchronize: process.env.NODE_ENV === 'development',
  logging: process.env.NODE_ENV === 'development',
};
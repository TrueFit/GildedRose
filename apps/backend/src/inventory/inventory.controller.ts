import { 
  Controller, 
  Get, 
  Post, 
  Param, 
  Body,
  HttpException, 
  HttpStatus,
  UseGuards
} from '@nestjs/common';
import { ApiTags, ApiOperation, ApiResponse, ApiParam, ApiBody } from '@nestjs/swagger';
import { 
  GetInventoryResponse, 
  GetItemResponse, 
  ProgressDayResponse, 
  GetTrashItemsResponse,
  CreateItemResponse,
  CreateSpecialItemRuleResponse,
  GetSpecialItemRulesResponse
} from '@gilded-rose/shared-types';
import { InventoryService } from './services/inventory.service';
import { DatabaseSeederService } from './services/database-seeder.service';
import { SpecialItemRuleService } from './services/special-item-rule.service';
import { CreateItemDto } from './dto/create-item.dto';
import { CreateSpecialItemRuleDto } from './dto/create-special-item-rule.dto';
import { GetAllItemsResponseDto } from './dto/get-all-items-response.dto';
import { GetItemByNameResponseDto } from './dto/get-item-by-name-response.dto';
import { ErrorResponseDto } from './dto/error-response.dto';
import { DevelopmentOnlyGuard } from './guards/development-only.guard';

@ApiTags('inventory')
@Controller('api/inventory')
export class InventoryController {
  constructor(
    private readonly inventoryService: InventoryService,
    private readonly seederService: DatabaseSeederService,
    private readonly specialItemRuleService: SpecialItemRuleService,
  ) {}

  @Get()
  @ApiOperation({ summary: 'Get all inventory items' })
  @ApiResponse({ 
    status: 200, 
    description: 'Returns all inventory items with stats',
    type: GetAllItemsResponseDto
  })
  @ApiResponse({ 
    status: 500, 
    description: 'Internal server error',
    type: ErrorResponseDto
  })
  async getAllItems(): Promise<GetInventoryResponse> {
    try {
      const items = await this.inventoryService.getAllItems();
      const stats = await this.inventoryService.calculateStats();
      
      return {
        success: true,
        data: items,
        stats,
        message: `Retrieved ${items.length} items from inventory`
      };
    } catch (error) {
      throw new HttpException(
        {
          success: false,
          error: 'Failed to retrieve inventory',
          message: error.message
        },
        HttpStatus.INTERNAL_SERVER_ERROR
      );
    }
  }

  @Get('item/:name')
  @ApiOperation({ summary: 'Get a specific item by name' })
  @ApiParam({ name: 'name', description: 'Item name', example: 'Sword' })
  @ApiResponse({ 
    status: 200, 
    description: 'Returns the requested item',
    type: GetItemByNameResponseDto
  })
  @ApiResponse({ 
    status: 404, 
    description: 'Item not found',
    type: ErrorResponseDto
  })
  @ApiResponse({ 
    status: 500, 
    description: 'Internal server error',
    type: ErrorResponseDto
  })
  async getItemByName(@Param('name') name: string): Promise<GetItemResponse> {
    try {
      const item = await this.inventoryService.getItemByName(name);
      
      return {
        success: true,
        data: item,
        message: `Found item: ${item.name}`
      };
    } catch (error) {
      if (error.message.includes('not found')) {
        throw new HttpException(
          {
            success: false,
            error: 'Item not found',
            message: error.message
          },
          HttpStatus.NOT_FOUND
        );
      }
      
      throw new HttpException(
        {
          success: false,
          error: 'Failed to retrieve item',
          message: error.message
        },
        HttpStatus.INTERNAL_SERVER_ERROR
      );
    }
  }

  @Post('progress-day')
  @ApiOperation({ summary: 'Progress to the next day' })
  @ApiResponse({ status: 200, description: 'Successfully progressed to next day' })
  async progressDay(): Promise<ProgressDayResponse> {
    try {
      const result = await this.inventoryService.progressToNextDay();
      
      return {
        success: true,
        data: result,
        message: result.message
      };
    } catch (error) {
      throw new HttpException(
        {
          success: false,
          error: 'Failed to progress day',
          message: error.message
        },
        HttpStatus.INTERNAL_SERVER_ERROR
      );
    }
  }

  @Get('trash')
  @ApiOperation({ summary: 'Get all items that should be thrown away' })
  @ApiResponse({ status: 200, description: 'Returns all items with quality = 0' })
  async getTrashItems(): Promise<GetTrashItemsResponse> {
    try {
      const trashItems = await this.inventoryService.getTrashItems();
      
      return {
        success: true,
        data: trashItems,
        message: `Found ${trashItems.length} items to throw away`
      };
    } catch (error) {
      throw new HttpException(
        {
          success: false,
          error: 'Failed to retrieve trash items',
          message: error.message
        },
        HttpStatus.INTERNAL_SERVER_ERROR
      );
    }
  }

  @Get('stats')
  @ApiOperation({ summary: 'Get inventory statistics' })
  @ApiResponse({ status: 200, description: 'Returns inventory statistics' })
  async getStats() {
    try {
      const stats = await this.inventoryService.calculateStats();
      
      return {
        success: true,
        data: stats,
        message: 'Statistics retrieved successfully'
      };
    } catch (error) {
      throw new HttpException(
        {
          success: false,
          error: 'Failed to retrieve statistics',
          message: error.message
        },
        HttpStatus.INTERNAL_SERVER_ERROR
      );
    }
  }

  @Get('database/status')
  @UseGuards(DevelopmentOnlyGuard)
  @ApiOperation({ 
    summary: 'Check database seeding status (Development Only)',
    description: 'This endpoint is only for this code challenge.'
  })
  @ApiResponse({ status: 200, description: 'Returns database seeding status' })
  @ApiResponse({ 
    status: 403, 
    description: 'Forbidden - Only available in development mode',
    type: ErrorResponseDto
  })
  async getDatabaseStatus() {
    try {
      const status = await this.seederService.getDatabaseStatus();
      const seedingRequired = await this.seederService.checkSeedingRequired();
      
      return {
        success: true,
        data: {
          ...status,
          ...seedingRequired,
        },
        message: status.isSeeded ? 'Database is seeded' : 'Database needs seeding'
      };
    } catch (error) {
      throw new HttpException(
        {
          success: false,
          error: 'Failed to check database status',
          message: error.message
        },
        HttpStatus.INTERNAL_SERVER_ERROR
      );
    }
  }

  @Post('database/reseed')
  @UseGuards(DevelopmentOnlyGuard)
  @ApiOperation({ 
    summary: 'Clear and reseed the database (Development Only)',
    description: 'This endpoint is only for this code challenge. It will clear all data and reseed the database.'
  })
  @ApiResponse({ status: 200, description: 'Database reseeded successfully' })
  @ApiResponse({ 
    status: 403, 
    description: 'Forbidden - Only available in development mode',
    type: ErrorResponseDto
  })
  async reseedDatabase() {
    try {
      await this.seederService.reseedDatabase();
      const status = await this.seederService.getDatabaseStatus();
      
      return {
        success: true,
        data: status,
        message: 'Database reseeded successfully'
      };
    } catch (error) {
      throw new HttpException(
        {
          success: false,
          error: 'Failed to reseed database',
          message: error.message
        },
        HttpStatus.INTERNAL_SERVER_ERROR
      );
    }
  }

  @Post('items')
  @ApiOperation({ summary: 'Add a new item to inventory' })
  @ApiBody({ 
    type: CreateItemDto,
    description: 'Item data to create',
    examples: {
      sword: {
        summary: 'A basic sword',
        value: {
          name: 'Iron Sword',
          category: 'Weapon',
          sellIn: 30,
          quality: 45
        }
      },
      potion: {
        summary: 'A health potion',
        value: {
          name: 'Health Potion',
          category: 'Potion',
          sellIn: 10,
          quality: 25
        }
      }
    }
  })
  @ApiResponse({ status: 201, description: 'Item created successfully' })
  @ApiResponse({ status: 400, description: 'Invalid item data or validation failed' })
  @ApiResponse({ status: 409, description: 'Item already exists' })
  async createItem(@Body() createItemDto: CreateItemDto): Promise<CreateItemResponse> {
    try {
      try {
        await this.inventoryService.getItemByName(createItemDto.name);
        throw new HttpException(
          {
            success: false,
            error: 'Item already exists',
            message: `An item with the name '${createItemDto.name}' already exists in inventory`
          },
          HttpStatus.CONFLICT
        );
      } catch (error) {
        // If item not found, that's what we want - continue with creation
        if (error.status !== HttpStatus.NOT_FOUND) {
          throw error;
        }
      }

      // Validate item against special item rules
      const validationResult = await this.specialItemRuleService.validateItem(createItemDto);
      
      if (!validationResult.isValid) {
        throw new HttpException(
          {
            success: false,
            error: 'Validation failed',
            message: `Item validation failed: ${validationResult.errors.join(', ')}`
          },
          HttpStatus.BAD_REQUEST
        );
      }

      const newItem = await this.inventoryService.addItem(createItemDto);
      
      return {
        success: true,
        data: newItem,
        message: `Successfully added '${newItem.name}' to inventory`
      };
    } catch (error) {
      if (error instanceof HttpException) {
        throw error;
      }
      
      throw new HttpException(
        {
          success: false,
          error: 'Failed to create item',
          message: error.message
        },
        HttpStatus.INTERNAL_SERVER_ERROR
      );
    }
  }

  @Get('special-item-rules')
  @ApiOperation({ summary: 'Get all special item rules' })
  @ApiResponse({ status: 200, description: 'Returns all special item rules' })
  async getAllSpecialItemRules(): Promise<GetSpecialItemRulesResponse> {
    try {
      const rules = await this.specialItemRuleService.getAllSpecialItemRules();
      
      return {
        success: true,
        data: rules,
        message: `Retrieved ${rules.length} special item rules`
      };
    } catch (error) {
      throw new HttpException(
        {
          success: false,
          error: 'Failed to retrieve special item rules',
          message: error.message
        },
        HttpStatus.INTERNAL_SERVER_ERROR
      );
    }
  }

  @Get('special-item-rules/:name')
  @ApiOperation({ summary: 'Get special item rule for specific item' })
  @ApiParam({ name: 'name', description: 'Name of the item', example: 'Aged Brie' })
  @ApiResponse({ status: 200, description: 'Returns the special item rule' })
  @ApiResponse({ status: 404, description: 'Special item rule not found' })
  async getSpecialItemRuleByName(@Param('name') name: string): Promise<CreateSpecialItemRuleResponse> {
    try {
      const rule = await this.specialItemRuleService.getSpecialItemRuleByName(name);
      
      return {
        success: true,
        data: rule,
        message: `Found special item rule for '${name}'`
      };
    } catch (error) {
      if (error.message.includes('not found')) {
        throw new HttpException(
          {
            success: false,
            error: 'Special item rule not found',
            message: error.message
          },
          HttpStatus.NOT_FOUND
        );
      }
      
      throw new HttpException(
        {
          success: false,
          error: 'Failed to retrieve special item rule',
          message: error.message
        },
        HttpStatus.INTERNAL_SERVER_ERROR
      );
    }
  }

  @Post('special-item-rules')
  @ApiOperation({ summary: 'Create or update a special item rule' })
  @ApiBody({ 
    type: CreateSpecialItemRuleDto,
    description: 'Special item rule data',
    examples: {
      agedBrie: {
        summary: 'Aged Brie special item rule',
        value: {
          name: 'Aged Brie',
          category: 'Food',
          ruleType: 'AGED_BRIE',
          minQuality: 0,
          maxQuality: 50,
          description: 'Aged Brie increases in quality over time'
        }
      },
      sulfuras: {
        summary: 'Sulfuras special item rule',
        value: {
          name: 'Sulfuras, Hand of Ragnaros',
          category: 'Sulfuras',
          ruleType: 'SULFURAS',
          allowedQualityValues: [80],
          description: 'Sulfuras is legendary and never changes'
        }
      }
    }
  })
  @ApiResponse({ status: 201, description: 'Special item rule created or updated successfully' })
  @ApiResponse({ status: 400, description: 'Invalid special item rule data' })
  async createOrUpdateSpecialItemRule(@Body() createRuleDto: CreateSpecialItemRuleDto): Promise<CreateSpecialItemRuleResponse> {
    try {
      const rule = await this.specialItemRuleService.createOrUpdateSpecialItemRule(createRuleDto);
      
      return {
        success: true,
        data: rule,
        message: `Successfully created/updated special item rule for '${rule.name}'`
      };
    } catch (error) {
      throw new HttpException(
        {
          success: false,
          error: 'Failed to create/update special item rule',
          message: error.message
        },
        HttpStatus.INTERNAL_SERVER_ERROR
      );
    }
  }
}
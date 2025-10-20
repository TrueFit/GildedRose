import { Controller, Get } from '@nestjs/common';
import { AppService } from './app.service';
import { ApiOperation, ApiResponse, ApiTags } from '@nestjs/swagger';

/**
 * The purpose of the AppController is to handle health check requests and provide basic information about the API status.
 * This is useful for monitoring and ensuring the backend service is operational.
 */
@ApiTags('health')
@Controller()
export class AppController {
  constructor(private readonly appService: AppService) {}

  @Get()
  @ApiOperation({ summary: 'Health check endpoint' })
  @ApiResponse({ status: 200, description: 'Service is healthy' })
  getHello(): string {
    return this.appService.getHello();
  }

  @Get('health')
  @ApiOperation({ summary: 'Health status' })
  @ApiResponse({ status: 200, description: 'Service health status' })
  getHealth(): { status: string; timestamp: string } {
    return this.appService.getHealth();
  }
}
import { Injectable, CanActivate, ExecutionContext, ForbiddenException } from '@nestjs/common';

@Injectable()
export class DevelopmentOnlyGuard implements CanActivate {
  canActivate(context: ExecutionContext): boolean {
    const isDevelopment = process.env.NODE_ENV === 'development' || !process.env.NODE_ENV;
    
    if (!isDevelopment) {
      throw new ForbiddenException('This endpoint is only available in development mode');
    }
    
    return true;
  }
}
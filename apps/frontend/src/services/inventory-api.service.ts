import { CreateItemDto, CreateItemResponse } from '@gilded-rose/shared-types';

const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:3001';

export class InventoryApiService {
  private static async request<T>(
    endpoint: string,
    options: RequestInit = {}
  ): Promise<T> {
    const url = `${API_BASE_URL}${endpoint}`;
    
    const response = await fetch(url, {
      headers: {
        'Content-Type': 'application/json',
        ...options.headers,
      },
      ...options,
    });

    if (!response.ok) {
      const errorData = await response.json().catch(() => ({}));
      throw new Error(errorData.message || `HTTP error! status: ${response.status}`);
    }

    return response.json();
  }

  static async createItem(itemData: CreateItemDto): Promise<CreateItemResponse> {
    return this.request<CreateItemResponse>('/api/inventory/items', {
      method: 'POST',
      body: JSON.stringify(itemData),
    });
  }
}
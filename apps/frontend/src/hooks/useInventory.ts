import { useState, useEffect } from 'react';
import { Item, InventoryStats, ItemCategory, CreateItemDto, CreateItemResponse } from '@gilded-rose/shared-types';
import { inventoryApi } from '@/lib/api';
import { InventoryApiService } from '@/services/inventory-api.service';

export const useInventory = () => {
  const [items, setItems] = useState<Item[]>([]);
  const [stats, setStats] = useState<InventoryStats | null>(null);
  const [loading, setLoading] = useState(true);
  const [progressingDay, setProgressingDay] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const loadInventory = async () => {
    try {
      setLoading(true);
      setError(null);
      const response = await inventoryApi.getAllItems();
      
      if (response.success && response.data) {
        setItems(response.data);
        setStats(response.stats || null);
      } else {
        setError(response.error || 'Failed to load inventory');
      }
    } catch (err) {
      setError('Failed to connect to the server');
      console.error('Failed to load inventory:', err);
    } finally {
      setLoading(false);
    }
  };

  const progressToNextDay = async () => {
    try {
      setProgressingDay(true);
      setError(null);
      const response = await inventoryApi.progressDay();
      
      if (response.success && response.data) {
        setItems(response.data.updatedItems);
        setStats(response.data.stats);
        // Show success message briefly
        setTimeout(() => setProgressingDay(false), 1000);
      } else {
        setError(response.error || 'Failed to progress day');
        setProgressingDay(false);
      }
    } catch (err) {
      setError('Failed to progress day');
      setProgressingDay(false);
      console.error('Failed to progress day:', err);
    }
  };

  const showTrashItems = async () => {
    try {
      setLoading(true);
      const response = await inventoryApi.getTrashItems();
      
      if (response.success && response.data) {
        setItems(response.data);
      }
    } catch (err) {
      setError('Failed to load trash items');
    } finally {
      setLoading(false);
    }
  };

  const createItem = async (itemData: CreateItemDto): Promise<CreateItemResponse> => {
    try {
      setError(null);
      const response = await InventoryApiService.createItem(itemData);
      
      // Reload inventory to get the updated list
      await loadInventory();
      
      return response;
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'Failed to create item';
      setError(errorMessage);
      throw new Error(errorMessage);
    }
  };

  const clearError = () => setError(null);

  // Load initial data
  useEffect(() => {
    loadInventory();
  }, []);

  return {
    items,
    stats,
    loading,
    progressingDay,
    error,
    loadInventory,
    progressToNextDay,
    showTrashItems,
    createItem,
    clearError,
  };
};
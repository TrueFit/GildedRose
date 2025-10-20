import axios from 'axios';
import { 
  GetInventoryResponse, 
  GetItemResponse, 
  ProgressDayResponse, 
  GetTrashItemsResponse,
  InventoryStats 
} from '@gilded-rose/shared-types';

const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:3001';

const api = axios.create({
  baseURL: API_BASE_URL,
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor for logging for debugging purposes
api.interceptors.request.use(
  (config) => {
    console.log(`API Request: ${config.method?.toUpperCase()} ${config.url}`);
    return config;
  },
  (error) => {
    console.error('API Request Error:', error);
    return Promise.reject(error);
  }
);

// Response interceptor for error handling and debugging
api.interceptors.response.use(
  (response) => {
    console.log(`API Response: ${response.status} ${response.config.url}`);
    return response;
  },
  (error) => {
    console.error('API Response Error:', error.response?.data || error.message);
    return Promise.reject(error);
  }
);

export const inventoryApi = {
  getAllItems: async (): Promise<GetInventoryResponse> => {
    const response = await api.get<GetInventoryResponse>('/api/inventory');
    return response.data;
  },
  getItemByName: async (name: string): Promise<GetItemResponse> => {
    const response = await api.get<GetItemResponse>(`/api/inventory/item/${encodeURIComponent(name)}`);
    return response.data;
  },
  progressDay: async (): Promise<ProgressDayResponse> => {
    const response = await api.post<ProgressDayResponse>('/api/inventory/progress-day');
    return response.data;
  },
  getTrashItems: async (): Promise<GetTrashItemsResponse> => {
    const response = await api.get<GetTrashItemsResponse>('/api/inventory/trash');
    return response.data;
  },
  getStats: async (): Promise<{ success: boolean; data: InventoryStats; message: string }> => {
    const response = await api.get('/api/inventory/stats');
    return response.data;
  },
};

export default api;
import React, { useState } from 'react';
import { Plus, X } from 'lucide-react';
import { ItemCategory, CreateItemDto } from '@gilded-rose/shared-types';

interface AddItemModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSubmit: (itemData: CreateItemDto) => Promise<any>;
}

export const AddItemModal: React.FC<AddItemModalProps> = ({
  isOpen,
  onClose,
  onSubmit,
}) => {
  const [formData, setFormData] = useState<CreateItemDto>({
    name: '',
    category: ItemCategory.MISC,
    sellIn: 30,
    quality: 20,
  });

  const [errors, setErrors] = useState<string[]>([]);
  const [loading, setLoading] = useState(false);

  const categories = Object.values(ItemCategory);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setErrors([]);

    const newErrors: string[] = [];
    
    if (!formData.name.trim()) {
      newErrors.push('Item name is required');
    }
    
    if (formData.quality < 0 || formData.quality > 80) {
      newErrors.push('Quality must be between 0 and 80');
    }
    
    if (formData.sellIn < -100 || formData.sellIn > 1000) {
      newErrors.push('Sell In must be between -100 and 1000');
    }

    if (newErrors.length > 0) {
      setErrors(newErrors);
      return;
    }

    try {
      setLoading(true);
      await onSubmit(formData);
 
      setFormData({
        name: '',
        category: ItemCategory.MISC,
        sellIn: 30,
        quality: 20,
      });
      onClose();
    } catch (error: any) {
      setErrors([error.message || 'Failed to create item']);
    } finally {
      setLoading(false);
    }
  };

  const handleInputChange = (field: keyof CreateItemDto, value: string | number) => {
    setFormData(prev => ({
      ...prev,
      [field]: value,
    }));

    if (errors.length > 0) {
      setErrors([]);
    }
  };

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center p-4 z-50">
      <div className="bg-white rounded-lg p-6 max-w-md w-full max-h-[90vh] overflow-y-auto">
        <div className="flex justify-between items-center mb-6">
          <h2 className="text-xl font-bold text-gray-900 flex items-center">
            <Plus className="h-5 w-5 mr-2" />
            Add New Product
          </h2>
          <button
            onClick={onClose}
            className="text-gray-400 hover:text-gray-600 transition-colors"
            disabled={loading}
          >
            <X className="h-6 w-6" />
          </button>
        </div>

        {errors.length > 0 && (
          <div className="mb-4 p-3 bg-red-50 border border-red-200 rounded-md">
            <div className="text-red-800 text-sm">
              <ul className="list-disc list-inside space-y-1">
                {errors.map((error, index) => (
                  <li key={index}>{error}</li>
                ))}
              </ul>
            </div>
          </div>
        )}

        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label htmlFor="itemName" className="block text-sm font-medium text-gray-700 mb-1">
              Product name *
            </label>
            <input
              id="itemName"
              type="text"
              value={formData.name}
              onChange={(e) => handleInputChange('name', e.target.value)}
              className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-blue-500 text-gray-900"
              placeholder="Enter item name..."
              required
              disabled={loading}
            />
          </div>

          <div>
            <label htmlFor="category" className="block text-sm font-medium text-gray-700 mb-1">
              Category *
            </label>
            <select
              id="category"
              value={formData.category}
              onChange={(e) => handleInputChange('category', e.target.value as ItemCategory)}
              className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-blue-500 text-gray-900 bg-white"
              required
              disabled={loading}
            >
              {categories.map(category => (
                <option key={category} value={category}>
                  {category}
                </option>
              ))}
            </select>
          </div>

          <div>
            <label htmlFor="quality" className="block text-sm font-medium text-gray-700 mb-1">
              Quality *
            </label>
            <input
              id="quality"
              type="number"
              min="0"
              max="80"
              value={formData.quality}
              onChange={(e) => handleInputChange('quality', parseInt(e.target.value) || 0)}
              className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-blue-500 text-gray-900"
              required
              disabled={loading}
            />
            <p className="mt-1 text-xs text-gray-500">
              Range: 0-50 for most items, 80 for Sulfuras items
            </p>
          </div>

          <div>
            <label htmlFor="sellIn" className="block text-sm font-medium text-gray-700 mb-1">
              Sell In (Days) *
            </label>
            <input
              id="sellIn"
              type="number"
              min="-100"
              max="1000"
              value={formData.sellIn}
              onChange={(e) => handleInputChange('sellIn', parseInt(e.target.value) || 0)}
              className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-blue-500 text-gray-900"
              required
              disabled={loading}
            />
            <p className="mt-1 text-xs text-gray-500">
              Days until item expires (can be negative)
            </p>
          </div>

          <div className="flex justify-end space-x-3 pt-4">
            <button
              type="button"
              onClick={onClose}
              className="px-4 py-2 border border-gray-300 rounded-md text-gray-700 hover:bg-gray-50 transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
              disabled={loading}
            >
              Cancel
            </button>
            <button
              type="submit"
              className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 transition-colors disabled:opacity-50 disabled:cursor-not-allowed flex items-center"
              disabled={loading}
            >
              {loading ? (
                <>
                  <div className="animate-spin rounded-full h-4 w-4 border-b-2 border-white mr-2"></div>
                  Creating new product...
                </>
              ) : (
                <>
                  <Plus className="h-4 w-4 mr-2" />
                  Create product
                </>
              )}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};
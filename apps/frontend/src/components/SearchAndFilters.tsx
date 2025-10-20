import React from 'react';
import { Search, Filter, Trash2, Plus } from 'lucide-react';
import { ItemCategory } from '@gilded-rose/shared-types';

interface SearchAndFiltersProps {
  searchTerm: string;
  onSearchChange: (value: string) => void;
  selectedCategory: ItemCategory | 'all';
  onCategoryChange: (category: ItemCategory | 'all') => void;
  showTrashOnly: boolean;
  onShowTrashOnly: () => void;
  onShowAll: () => void;
  onAddItem?: () => void;
}

export const SearchAndFilters: React.FC<SearchAndFiltersProps> = ({
  searchTerm,
  onSearchChange,
  selectedCategory,
  onCategoryChange,
  showTrashOnly,
  onShowTrashOnly,
  onShowAll,
  onAddItem,
}) => {
  const categories = Object.values(ItemCategory);

  return (
    <div className="mb-6 bg-white rounded-lg border border-gray-200 p-4">
      <div className="flex flex-col md:flex-row md:items-center md:justify-between space-y-4 md:space-y-0 md:space-x-4">
        {/* Search */}
        <div className="relative flex-1 max-w-md">
          <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400" />
          <input
            type="text"
            placeholder="Search products..."
            value={searchTerm}
            onChange={(e) => onSearchChange(e.target.value)}
            className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 text-gray-900 placeholder-gray-500"
          />
        </div>

        {/* Filters and Actions */}
        <div className="flex items-center space-x-4">
          <div className="flex items-center space-x-2">
            <Filter className="h-4 w-4 text-gray-500" />
            <select
              value={selectedCategory}
              onChange={(e) => onCategoryChange(e.target.value as ItemCategory | 'all')}
              className="border border-gray-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-blue-500 focus:border-blue-500 text-gray-900 bg-white"
            >
              <option value="all">All Categories</option>
              {categories.map(category => (
                <option key={category} value={category}>{category}</option>
              ))}
            </select>
          </div>

          <button
            onClick={onShowTrashOnly}
            className={`inline-flex items-center px-3 py-2 rounded-lg border transition-colors duration-200 ${
              showTrashOnly 
                ? 'bg-red-50 border-red-200 text-red-700' 
                : 'border-gray-300 text-gray-700 hover:bg-gray-50'
            }`}
          >
            <Trash2 className="h-4 w-4 mr-2" />
            Trash Only
          </button>

          {showTrashOnly && (
            <button
              onClick={onShowAll}
              className="text-sm text-blue-600 hover:text-blue-700"
            >
              Show All
            </button>
          )}

          {onAddItem && (
            <button
              onClick={onAddItem}
              className="inline-flex items-center px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 transition-colors duration-200"
            >
              <Plus className="h-4 w-4 mr-2" />
              Add Product
            </button>
          )}
        </div>
      </div>
    </div>
  );
};
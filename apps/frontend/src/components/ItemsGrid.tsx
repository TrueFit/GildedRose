import React from 'react';
import { Clock } from 'lucide-react';
import { Item, ItemCategory } from '@gilded-rose/shared-types';
import { ItemCard } from './ItemCard';

interface ItemsGridProps {
  items: Item[];
  searchTerm: string;
  selectedCategory: ItemCategory | 'all';
  onItemClick: (item: Item) => void;
}

export const ItemsGrid: React.FC<ItemsGridProps> = ({
  items,
  searchTerm,
  selectedCategory,
  onItemClick,
}) => {
  const filteredItems = items.filter(item => {
    const matchesSearch = item.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
                         item.category.toLowerCase().includes(searchTerm.toLowerCase());
    const matchesCategory = selectedCategory === 'all' || item.category === selectedCategory;
    return matchesSearch && matchesCategory;
  });

  if (filteredItems.length === 0) {
    return (
      <div className="text-center py-12 bg-white rounded-lg border border-gray-200">
        <Clock className="h-12 w-12 text-gray-400 mx-auto mb-4" />
        <h3 className="text-lg font-medium text-gray-900 mb-2">No Items Found</h3>
        <p className="text-gray-500">
          {searchTerm || selectedCategory !== 'all' 
            ? 'Try adjusting your search or filter criteria.'
            : 'The inventory appears to be empty.'}
        </p>
      </div>
    );
  }

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
      {filteredItems.map((item) => (
        <ItemCard
          key={item.id}
          item={item}
          onClick={() => onItemClick(item)}
        />
      ))}
    </div>
  );
};
import React from 'react';
import { Item } from '@gilded-rose/shared-types';

interface ItemModalProps {
  item: Item | null;
  onClose: () => void;
}

export const ItemModal: React.FC<ItemModalProps> = ({ item, onClose }) => {
  if (!item) return null;

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center p-4 z-50">
      <div className="bg-white rounded-lg p-6 max-w-md w-full">
        <div className="flex justify-between items-start mb-4">
          <h2 className="text-xl font-bold text-gray-900">{item.name}</h2>
          <button
            onClick={onClose}
            className="text-gray-400 hover:text-gray-600 text-xl"
          >
            ✕
          </button>
        </div>
        
        <div className="space-y-3">
          <div className="flex justify-between">
            <span className="font-medium">Category:</span>
            <span>{item.category}</span>
          </div>
          <div className="flex justify-between">
            <span className="font-medium">Quality:</span>
            <span>{item.quality}</span>
          </div>
          <div className="flex justify-between">
            <span className="font-medium">Sell In:</span>
            <span>{item.sellIn} days</span>
          </div>
          {item.createdAt && (
            <div className="flex justify-between">
              <span className="font-medium">Added:</span>
              <span>{new Date(item.createdAt).toLocaleDateString()}</span>
            </div>
          )}
          {item.updatedAt && (
            <div className="flex justify-between">
              <span className="font-medium">Updated:</span>
              <span>{new Date(item.updatedAt).toLocaleDateString()}</span>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};
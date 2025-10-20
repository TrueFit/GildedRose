import React, { useState, useEffect } from 'react';
import { Item, ItemCategory } from '@gilded-rose/shared-types';
import { Sword, Shield, Apple, Gem, Package, Zap, FlaskConical, Shirt } from 'lucide-react';

interface ItemCardProps {
  item: Item;
  onClick?: () => void;
}

const getCategoryIcon = (category: ItemCategory) => {
  const iconProps = { size: 20, className: "text-gray-600" };
  
  switch (category) {
    case ItemCategory.WEAPON:
      return <Sword {...iconProps} />;
    case ItemCategory.ARMOR:
      return <Shield {...iconProps} />;
    case ItemCategory.FOOD:
      return <Apple {...iconProps} />;
    case ItemCategory.SULFURAS:
      return <Gem {...iconProps} className="text-orange-500" />;
    case ItemCategory.BACKSTAGE_PASSES:
      return <Package {...iconProps} className="text-purple-500" />;
    case ItemCategory.CONJURED:
      return <Zap {...iconProps} className="text-blue-500" />;
    case ItemCategory.POTION:
      return <FlaskConical {...iconProps} className="text-green-500" />;
    case ItemCategory.MISC:
    default:
      return <Shirt {...iconProps} />;
  }
};

const getQualityColor = (quality: number, maxQuality: number = 50) => {
  const percentage = (quality / maxQuality) * 100;
  
  if (percentage >= 80) return 'text-green-600 bg-green-50';
  if (percentage >= 60) return 'text-yellow-600 bg-yellow-50';
  if (percentage >= 40) return 'text-orange-600 bg-orange-50';
  if (percentage > 0) return 'text-red-600 bg-red-50';
  return 'text-gray-500 bg-gray-100';
};

const getSellInColor = (sellIn: number) => {
  if (sellIn < 0) return 'text-red-600 bg-red-50';
  if (sellIn <= 5) return 'text-orange-600 bg-orange-50';
  if (sellIn <= 10) return 'text-yellow-600 bg-yellow-50';
  return 'text-green-600 bg-green-50';
};

export const ItemCard: React.FC<ItemCardProps> = ({ item, onClick }) => {
  const [isAnimating, setIsAnimating] = useState(false);

  useEffect(() => {
    setIsAnimating(true);
    const timer = setTimeout(() => setIsAnimating(false), 300);
    return () => clearTimeout(timer);
  }, [item.quality, item.sellIn]);

  const qualityColorClass = getQualityColor(item.quality);
  const sellInColorClass = getSellInColor(item.sellIn);

  return (
    <div
      className={`
        bg-white rounded-lg border border-gray-200 p-4 shadow-sm hover:shadow-md 
        transition-all duration-200 cursor-pointer hover:border-gray-300
        ${isAnimating ? 'animate-pulse' : ''}
        ${onClick ? 'hover:scale-[1.02]' : ''}
      `}
      onClick={onClick}
    >
      <div className="flex items-start justify-between mb-3">
        <div className="flex items-center space-x-2">
          {getCategoryIcon(item.category)}
          <div>
            <h3 className="font-semibold text-gray-900 text-sm truncate max-w-[200px]" title={item.name}>
              {item.name}
            </h3>
            <p className="text-xs text-gray-500">{item.category}</p>
          </div>
        </div>
        
        {item.category === ItemCategory.SULFURAS && (
          <span className="inline-flex items-center px-2 py-1 rounded-full text-xs font-medium bg-orange-100 text-orange-800">
            Legendary
          </span>
        )}
      </div>

      <div className="space-y-2">
        <div className="flex justify-between items-center">
          <span className="text-sm font-medium text-gray-700">Quality:</span>
          <span className={`px-2 py-1 rounded-full text-xs font-semibold ${qualityColorClass}`}>
            {item.quality}
            {item.category === ItemCategory.SULFURAS ? '' : '/50'}
          </span>
        </div>
        
        <div className="flex justify-between items-center">
          <span className="text-sm font-medium text-gray-700">Sell In:</span>
          <span className={`px-2 py-1 rounded-full text-xs font-semibold ${sellInColorClass}`}>
            {item.sellIn > 0 ? `${item.sellIn} days` : item.sellIn === 0 ? 'Today!' : 'Expired'}
          </span>
        </div>
      </div>

      <div className="mt-3">
        <div className="w-full bg-gray-200 rounded-full h-2">
          <div
            className={`h-2 rounded-full transition-all duration-300 ${
              item.quality > 40 ? 'bg-green-500' :
              item.quality > 20 ? 'bg-yellow-500' :
              item.quality > 0 ? 'bg-orange-500' : 'bg-red-500'
            }`}
            style={{ 
              width: `${Math.min((item.quality / (item.category === ItemCategory.SULFURAS ? 80 : 50)) * 100, 100)}%` 
            }}
          />
        </div>
      </div>

      {item.quality === 0 && (
        <div className="mt-2 text-center">
          <span className="inline-flex items-center px-2 py-1 rounded-full text-xs font-medium bg-red-100 text-red-800">
            🗑️ Trash
          </span>
        </div>
      )}
      
      {item.sellIn <= 5 && item.sellIn > 0 && item.quality > 0 && (
        <div className="mt-2 text-center">
          <span className="inline-flex items-center px-2 py-1 rounded-full text-xs font-medium bg-orange-100 text-orange-800">
            ⚠️ Expiring Soon
          </span>
        </div>
      )}
    </div>
  );
};
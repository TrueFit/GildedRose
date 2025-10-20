import React from 'react';
import { InventoryStats as IInventoryStats, ItemCategory } from '@gilded-rose/shared-types';
import { BarChart, TrendingUp, AlertTriangle, Trash2 } from 'lucide-react';

interface InventoryStatsProps {
  stats: IInventoryStats;
}

export const InventoryStats: React.FC<InventoryStatsProps> = ({ stats }) => {
  const statCards = [
    {
      title: 'Total Products',
      value: stats.totalItems,
      icon: <BarChart size={20} />,
      color: 'bg-blue-50 text-blue-600',
      iconColor: 'text-blue-500',
    },
    {
      title: 'Avg Quality',
      value: stats.averageQuality.toFixed(1),
      icon: <TrendingUp size={20} />,
      color: 'bg-green-50 text-green-600',
      iconColor: 'text-green-500',
    },
    {
      title: 'Expiring Soon',
      value: stats.itemsExpiringSoon,
      icon: <AlertTriangle size={20} />,
      color: 'bg-orange-50 text-orange-600',
      iconColor: 'text-orange-500',
    },
    {
      title: 'Trash products',
      value: stats.trashItems,
      icon: <Trash2 size={20} />,
      color: 'bg-red-50 text-red-600',
      iconColor: 'text-red-500',
    },
  ];

  return (
    <div className="grid grid-cols-1 lg:grid-cols-4 gap-6">
      <div className="lg:col-span-1">
        <div className="grid grid-cols-1 gap-4">
          {statCards.map((stat, index) => (
            <div key={index} className="bg-white rounded-lg border border-gray-200 p-4 shadow-sm">
              <div className="flex items-center justify-between">
                <div>
                  <p className="text-sm font-medium text-gray-600">{stat.title}</p>
                  <p className={`text-2xl font-bold ${stat.color.split(' ')[1]}`}>
                    {stat.value}
                  </p>
                </div>
                <div className={`p-2 rounded-lg ${stat.color.split(' ')[0]} ${stat.iconColor}`}>
                  {stat.icon}
                </div>
              </div>
            </div>
          ))}
        </div>
      </div>

      <div className="lg:col-span-3">
        <div className="bg-white rounded-lg border border-gray-200 p-6 shadow-sm h-full">
          <h3 className="text-lg font-semibold text-gray-900 mb-4">Products by Category</h3>
          <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
            {Object.entries(stats.itemsByCategory).map(([category, count]) => {
              const percentage = stats.totalItems > 0 ? (count / stats.totalItems) * 100 : 0;
              
              return (
                <div key={category} className="space-y-2">
                  <div className="flex items-center justify-between">
                    <span className="text-sm font-medium text-gray-700">
                      {category}
                    </span>
                    <div className="text-right">
                      <span className="text-sm font-semibold text-gray-900">{count}</span>
                      <span className="text-xs text-gray-500 ml-1">
                        ({percentage.toFixed(1)}%)
                      </span>
                    </div>
                  </div>
                  <div className="bg-gray-200 rounded-full h-2">
                    <div
                      className="bg-blue-500 h-2 rounded-full transition-all duration-300"
                      style={{ width: `${percentage}%` }}
                    />
                  </div>
                </div>
              );
            })}
          </div>
        </div>
        
      </div>
    </div>
  );
};
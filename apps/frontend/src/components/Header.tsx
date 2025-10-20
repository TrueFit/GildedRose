import React from 'react';
import { RefreshCw, Calendar, Store } from 'lucide-react';

interface HeaderProps {
  onProgressDay: () => void;
  onRefresh: () => void;
  progressingDay: boolean;
  loading: boolean;
}

export const Header: React.FC<HeaderProps> = ({
  onProgressDay,
  onRefresh,
  progressingDay,
  loading,
}) => {
  return (
    <header className="bg-white shadow-sm border-b">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex justify-between items-center h-16">
          <div className="flex items-center space-x-3">
            <Store className="h-8 w-8 text-blue-600" />
            <div>
              <h1 className="text-2xl font-bold text-gray-900">Gilded Rose</h1>
              <p className="text-sm text-gray-500">Inn Management</p>
            </div>
          </div>
          
          <div className="flex items-center space-x-4">
            <button
              onClick={onProgressDay}
              disabled={progressingDay}
              className="inline-flex items-center px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed transition-colors duration-200"
            >
              {progressingDay ? (
                <RefreshCw className="animate-spin h-4 w-4 mr-2" />
              ) : (
                <Calendar className="h-4 w-4 mr-2" />
              )}
              {progressingDay ? 'Updatring...' : 'Update to Next Day'}
            </button>
            
            <button
              onClick={onRefresh}
              disabled={loading}
              className="inline-flex items-center px-3 py-2 border border-gray-300 rounded-lg text-gray-700 hover:bg-gray-50 disabled:opacity-50 transition-colors duration-200"
            >
              <RefreshCw className={`h-4 w-4 ${loading ? 'animate-spin' : ''}`} />
            </button>
          </div>
        </div>
      </div>
    </header>
  );
};
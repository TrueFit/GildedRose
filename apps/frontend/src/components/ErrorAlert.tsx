import React from 'react';
import { AlertCircle } from 'lucide-react';

interface ErrorAlertProps {
  error: string | null;
  onDismiss: () => void;
}

export const ErrorAlert: React.FC<ErrorAlertProps> = ({ error, onDismiss }) => {
  if (!error) return null;

  return (
    <div className="mb-6 bg-red-50 border border-red-200 rounded-lg p-4">
      <div className="flex items-center">
        <AlertCircle className="h-5 w-5 text-red-500 mr-2" />
        <p className="text-red-700">{error}</p>
        <button
          onClick={onDismiss}
          className="ml-auto text-red-500 hover:text-red-700"
        >
          ✕
        </button>
      </div>
    </div>
  );
};
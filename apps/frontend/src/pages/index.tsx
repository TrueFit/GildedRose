import React, { useState } from 'react';
import Head from 'next/head';
import { Item, ItemCategory } from '@gilded-rose/shared-types';
import { useInventory } from '@/hooks/useInventory';
import { Header } from '@/components/Header';
import { SearchAndFilters } from '@/components/SearchAndFilters';
import { ItemsGrid } from '@/components/ItemsGrid';
import { ItemModal } from '@/components/ItemModal';
import { AddItemModal } from '@/components/AddItemModal';
import { ErrorAlert } from '@/components/ErrorAlert';
import { LoadingSpinner } from '@/components/LoadingSpinner';
import { InventoryStats } from '@/components/InventoryStats';

export default function Home() {
  // Inventory state and actions
  const {
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
  } = useInventory();

  // UI state
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedCategory, setSelectedCategory] = useState<ItemCategory | 'all'>('all');
  const [showTrashOnly, setShowTrashOnly] = useState(false);
  const [selectedItem, setSelectedItem] = useState<Item | null>(null);
  const [showAddItemModal, setShowAddItemModal] = useState(false);

  // Handle showing trash items
  const handleShowTrashOnly = async () => {
    await showTrashItems();
    setShowTrashOnly(true);
  };

  // Handle showing all items
  const handleShowAll = () => {
    setShowTrashOnly(false);
    loadInventory();
  };

  // Loading state
  if (loading && items.length === 0) {
    return <LoadingSpinner message="Loading Gilded Rose inventory..." />;
  }

  return (
    <>
      <Head>
        <title>Gilded Rose - Inventory Management</title>
        <meta name="description" content="Manage the Gilded Rose inn inventory" />
        <meta name="viewport" content="width=device-width, initial-scale=1" />
        <link rel="icon" href="/favicon.ico" />
      </Head>

      <div className="min-h-screen bg-gray-50">
        <Header
          onProgressDay={progressToNextDay}
          onRefresh={loadInventory}
          progressingDay={progressingDay}
          loading={loading}
        />

        <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
          <ErrorAlert error={error} onDismiss={clearError} />

          {stats && (
            <div className="mb-8">
              <InventoryStats stats={stats} />
            </div>
          )}

          <SearchAndFilters
            searchTerm={searchTerm}
            onSearchChange={setSearchTerm}
            selectedCategory={selectedCategory}
            onCategoryChange={setSelectedCategory}
            showTrashOnly={showTrashOnly}
            onShowTrashOnly={handleShowTrashOnly}
            onShowAll={handleShowAll}
            onAddItem={() => setShowAddItemModal(true)}
          />

          <ItemsGrid
            items={items}
            searchTerm={searchTerm}
            selectedCategory={selectedCategory}
            onItemClick={setSelectedItem}
          />
        </main>

        <ItemModal
          item={selectedItem}
          onClose={() => setSelectedItem(null)}
        />

        <AddItemModal
          isOpen={showAddItemModal}
          onClose={() => setShowAddItemModal(false)}
          onSubmit={createItem}
        />
      </div>
    </>
  );
}
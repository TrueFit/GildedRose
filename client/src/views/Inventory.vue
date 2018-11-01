<template>
  <div class="inventory">
    <menu class="inventory__menu">
      <label for="filter">Showing</label>
      <select id="filter" @change="onFilter">
        <option value="all">All Items</option>
        <option value="trash">Items for Trash</option>
      </select>
    </menu>

    <InventoryList :filter="filter" />
  </div>
</template>

<script>
import InventoryList from '../components/InventoryList';

export default {
  components: { InventoryList },

  data: () => ({ filter: {} }),

  methods: {
    onFilter(evt) {
      const filter = evt.target.value;
      if (filter === 'trash') {
        this.filter = { quality: 0 };
      } else {
        this.filter = {};
      }
    }
  }
};
</script>

<style lang="stylus">
.inventory
  max-width 30em
  margin 0 auto

.inventory__menu
  text-align center
  padding 0

.inventory__menu label
  margin-right 0.5em
  
.inventory__menu select
  font-size 1em
  font inherit
</style>

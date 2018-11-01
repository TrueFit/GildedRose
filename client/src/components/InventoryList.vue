<template>
  <div class="inventory-list">
    <p
      class="inventory-list__loading inventory-list__text"
      v-if="isFetching"
    >
      Fetching inventory...
    </p>

    <p
      class="inventory-list__error inventory-list__text"
      v-if="error"
    >
      {{ error }}
    </p>

    <p
      class="inventory-list__empty inventory-list__text"
      v-if="inventory && inventory.length === 0"
    >
      No items!
    </p>

    <div class="inventory-list__list" v-if="inventory && inventory.length > 0">
      <table>
        <thead>
          <tr>
            <th>Name</th>
            <th>Category</th>
            <th class="number">Sell In</th>
            <th class="number">Quality</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(item, index) in inventory" :key="index">
            <td>{{ item.name }}</td>
            <td>{{ item.category }}</td>
            <td class="number">{{ item.sellIn }}</td>
            <td class="number">{{ item.quality }}</td>
          </tr>
        </tbody>
      </table>
    </div> 
  </div>
</template>

<script>
import { find } from "../services/Inventory";

export default {
  data() {
    return {
      error: null,
      inventory: null,
      isFetching: false,
    };
  },

  props: {
    'filter': { type: Object, default: () => ({}) },
  },

  mounted() {
    this.fetchInventory();
  },

  methods: {
    async fetchInventory() {
      this.isFetching = true;

      try {
        this.inventory = await find(this.filter);
        this.error = null;
        this.isFetching = false;
      }
      catch (error) {
        this.inventory = null;
        this.error = error;
        this.isFetching = false;
      }
    }
  },

  watch: {
    filter() {
      this.fetchInventory();
    }
  }
};
</script>

<style lang="stylus">
.inventory-list__list table
  width 100%

.inventory-list__list th, .inventory-list__list td
  text-align left

  &:not(:last-child)
    padding-right 1em

.inventory-list__list .number
  text-align right

.inventory-list__text
  text-align center

.inventory-list__error
  color firebrick
  font-weight 700
</style>

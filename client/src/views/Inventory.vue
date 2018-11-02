<template>
  <div class="inventory">
    <div class="inventory__menu">
      <label for="filter">Showing</label>
      <select id="filter" @change="onFilter">
        <option value="all">All Items</option>
        <option value="trash">Items for Trash</option>
      </select>
    </div>

    <p class="inventory__message inventory__text" v-if="message">{{ message }}</p>
    <p class="inventory__error inventory__text" v-if="error">{{ error }}</p>

    <div class="inventory__content" v-if="!message && !error">
      <InventoryList :items="items" />
    </div>

    <div class="inventory__actions">
      <button 
        @click="advanceToNextDay"
        :disabled="error"
      >
        Advance to Next Day
      </button>
    </div>
  </div>
</template>

<script>
import InventoryList from '../components/InventoryList';
import { find, advance } from '../services/Inventory';

export default {
  components: { InventoryList },

  data: () => ({
    error: null,
    filter: {},
    items: [],
    message: null,
  }),

  mounted() {
    this.fetchItems();
  },

  methods: {
    async fetchItems() {
      this.message = 'Fetching inventory...';
      try {
        this.items = await find(this.filter);
        this.message = null;
      }
      catch (e) {
        this.message = null;
        this.error = 'There was a problem fetching the inventory. Please refresh to try again.';
      }
    },

    async advanceToNextDay() {
      this.message = 'Updating inventory...';
      try {
        await advance();
        this.message = null;
        this.fetchItems();
      }
      catch (e) {
        this.message = null;
        this.error = 'There was a problem updating the inventory. Please refresh and try again.';
      }
    },

    onFilter(evt) {
      const filter = evt.target.value;
      if (filter === 'trash') {
        this.filter = { quality: 0 };
      } else {
        this.filter = {};
      }
      this.fetchItems();
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
  margin-bottom 2em

  label
    margin-right 0.5em

  select
    font-size 1em
    font-family inherit

.inventory__text
  text-align center

.inventory__error
  color firebrick
  font-weight 700

.inventory__actions
  text-align center

  button
    background-color dodgerblue
    border 0
    font-size 1em
    font-family inherit
    color white
    padding 0.5em 1em
    border-radius 0.2em
    margin-top 3em

    &:active
      background-color darken(dodgerblue, 35%)
</style>

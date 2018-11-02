<template>
  <div class="inventory">
    <div class="inventory__menu">
      <label for="filter">Showing</label>
      <select id="filter" @change="onFilter">
        <option value="all">All Items</option>
        <option value="trash">Items for Trash</option>
      </select>
    </div>

    <p class="inventory__message" v-if="message">{{ message }}</p>
    <p class="inventory__error" v-if="error">{{ error }}</p>


    <InventoryList :filter="filter" />

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
import { advance } from '../services/Inventory';

export default {
  components: { InventoryList },

  data: () => ({
    error: null,
    filter: {},
    message: null,
  }),

  methods: {
    async advanceToNextDay() {
      this.message = 'Updating inventory...';

      try {
        await advance();
        this.message = null;
        this.filter = {}; // triggers the list to refresh
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

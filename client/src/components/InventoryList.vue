<template>
  <div class="inventory-list">
    <p
      class="inventory-list__empty inventory__text"
      v-if="items && items.length === 0"
    >
      No items!
    </p>

    <div class="inventory-list__content" v-if="items && items.length > 0">
      <!-- for small screens -->
      <ul class="inventory-list__list">
        <li
          v-for="(item, index) in items"
          :key="index"
          class="inventory-list__item"
        >
          <h3>
            <router-link :to="getPath(item)">{{ item.name }}</router-link>
          </h3>
          <Item :item="item" />
        </li>
      </ul>

      <!-- for big screens -->
      <table class="inventory-list__table">
        <thead>
          <tr>
            <th>Name</th>
            <th>Category</th>
            <th class="number">Sell In</th>
            <th class="number">Quality</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(item, index) in items" :key="index">
            <td>
              <router-link :to="getPath(item)">{{ item.name }}</router-link>
            </td>
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
import Item from './Item';
export default {
  components: { Item },
  props: ['items'],

  methods: {
    getPath(item) {
      return `/items/${encodeURIComponent(item.name)}`;
    }
  }
};
</script>

<style lang="stylus">
.inventory-list__table 
  display none
  width 100%

  th, td
    text-align left

    &:not(:last-child)
      padding-right 1em

  .number
    text-align right

.inventory-list__list
  list-style none
  padding 0
  text-align center

.inventory-list__item 
  &:not(:last-child)
    margin-bottom 2em

  h3
    margin-bottom 0

@media (min-width: 30em)
  .inventory-list__table
    display table

  .inventory-list__list
    display none
</style>

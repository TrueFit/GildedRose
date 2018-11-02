<template>
  <div class="inventory-list">
    <p
      class="inventory-list__empty inventory__text"
      v-if="items && items.length === 0"
    >
      No items!
    </p>

    <div class="inventory-list__list" v-if="items && items.length > 0">
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
          <tr v-for="(item, index) in items" :key="index">
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
export default {
  props: ['items'],
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
</style>

<template>
  <div class="item">
    <!-- very loosely based on https://refactoringui.com/previews/labels-are-a-last-resort/ -->
    <p class="item__data">{{ item.category }}</p>
    <p class="item__data">
      Quality: <span class="item__number">{{ item.quality }}</span>
    </p>
    <p class="item__data">
      Sell in <span class="item__number">{{ item.sellIn }}</span> days
    </p>
  </div>
</template>

<script>
export default {
  props: ['item'],
}
</script>

<style lang="stylus">
.item
  text-align center

.item__data
  margin 0

.item__number
  font-weight 700
  font-size 1.3em
</style>

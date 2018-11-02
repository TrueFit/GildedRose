<template>
  <div class="item-details">
    <h2>{{ $route.params.name }}</h2>

    <!-- this message/error/content pattern could be refactored to a
      template component -->
    <p
      class="item-details__message inventory__text"
      v-if="message"
    >
      {{ message }}
    </p>

    <p
      class="item-details__error inventory__error inventory__text"
      v-if="error"
    >
      {{ error }}
    </p>

    <div class="item-details__content" v-if="item">
      <!-- very loosely based on https://refactoringui.com/previews/labels-are-a-last-resort/ -->
      <p class="item-details__data">{{ item.category }}</p>
      <p class="item-details__data">
        Quality: <span class="item-details__number">{{ item.quality }}</span>
      </p>
      <p class="item-details__data">
        Sell in <span class="item-details__number">{{ item.sellIn }}</span> days
      </p>
    </div>

    <div class="item-details__actions">
      <router-link to="/">Back to List</router-link>
    </div>
  </div>
</template>

<script>
import { get } from '../services/Inventory';

export default {
  data() {
    return {
      error: null,
      item: null,
      message: null,
    };
  },

  mounted() {
    this.get(this.$route.params.name);
  },

  methods: {
    async get(name) {
      try {
        this.message = 'Fetching details...';
        this.item = await get(name);
        this.message = null;
      }
      catch (e) {
        this.message = null;
        this.item = null;
        this.error = e.message;
      }
    },
  },

  watch: {
    '$route' (to, from) {
      this.get(to.params.name);
    }
  }
}
</script>

<style lang="stylus">
.item-details
  text-align center

.item-details__data
  margin 0

.item-details__actions
  margin-top 3em

.item-details__number
  font-weight 700
  font-size 1.3em
</style>

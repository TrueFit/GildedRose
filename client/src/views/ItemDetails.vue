<template>
  <div class="item-details">
    <h2>{{ $route.params.name }}</h2>

    <!-- this message/error/content pattern also shows up in InventoryList
      and could be refactored to a template component -->
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
      <Item :item="item" />
    </div>

    <div class="item-details__actions">
      <router-link to="/">Back to List</router-link>
    </div>
  </div>
</template>

<script>
import { get } from '../services/Inventory';
import Item from '../components/Item';

export default {
  data() {
    return {
      error: null,
      item: null,
      message: null,
    };
  },

  components: { Item },

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
.item-details__actions
  margin-top 3em
</style>

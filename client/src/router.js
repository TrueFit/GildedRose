import Vue from "vue";
import Router from "vue-router";
import Inventory from "./views/Inventory.vue";

Vue.use(Router);

export default new Router({
  routes: [
    {
      path: "/",
      name: "inventory",
      component: Inventory
    },
    {
      path: "/item/:name",
      name: "itemdetails",
      // route level code-splitting
      // this generates a separate chunk (about.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () =>
        import(/* webpackChunkName: "about" */ "./views/ItemDetails.vue")
    }
  ]
});

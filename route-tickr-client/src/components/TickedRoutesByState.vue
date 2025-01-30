<script setup>
import { ref, computed } from "vue";
import { useStore } from "vuex";

const store = useStore();
const stateName = ref("");

const filteredTickedRoutes = computed(() => store.getters.filteredRoutes || []);

const fetchRoutes = () => {
  const stateKey = stateName.value;
  if (!stateKey) return;
  
  if (store.getters.tickedRoutesByState?.[stateKey]) {
    console.log(`Using cached ticked route IDs for ${stateKey}`);
    return;
  }
  
  //console.log("fetchingRoutes from store");
  store.dispatch("fetchRouteIdsByState", stateKey);
};
</script>

<template>
  <div class="container">
    <h1 class="title">Ticked Routes by State</h1>

    <!-- Input for entering the state name -->
    <div class="input-box">
      <input v-model="stateName" placeholder="Enter state name" @keyup.enter="fetchRoutes" />
      <button @click="fetchRoutes">Search</button>
    </div>

    <!-- Display Table if routes are found -->
    <table v-if="filteredTickedRoutes.length > 0" class="route-table">
      <thead>
      <tr>
        <th>Route</th>
        <th>Location</th>
        <th>Rating</th>
        <th>Average Stars</th>
        <th>Length (m)</th>
      </tr>
      </thead>
      <tbody>
      <tr v-for="route in filteredTickedRoutes" :key="route.id">
        <td>{{ route.route }}</td>
        <td>{{ route.location }}</td>
        <td>{{ route.rating }}</td>
        <td>{{ route.avgStars }}</td>
        <td>{{ route.length }}</td>
      </tr>
      </tbody>
    </table>

    <!-- Message if no routes are found -->
    <p v-else>No ticked routes found for this state.</p>
  </div>
</template>

<style scoped>
.container {
  max-width: 600px;
  margin: 20px auto;
  text-align: center;
}
.title {
  font-size: 24px;
  margin-bottom: 20px;
}
.input-box {
  margin-bottom: 20px;
}
input {
  padding: 8px;
  width: 200px;
  margin-right: 10px;
}
button {
  padding: 8px 15px;
  cursor: pointer;
}
.route-table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 20px;
}
.route-table th, .route-table td {
  border: 1px solid #ddd;
  padding: 8px;
}
.route-table th {
  background-color: #f4f4f4;
}
</style>
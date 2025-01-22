<script setup>
import { ref, computed, onMounted } from 'vue';
import { useStore } from 'vuex';

const store = useStore();

const allRoutes = computed(() => store.getters.allRoutes);
const currentPage = ref(1);
const itemsPerPage = 30;


const paginatedRoutes = computed(() => {
  if (!Array.isArray(allRoutes.value)) {
    return [];
  }
  const startIndex = (currentPage.value - 1) * itemsPerPage;
  return allRoutes.value.slice(startIndex, startIndex + itemsPerPage);
});

onMounted(async () => {
  try {
    await store.dispatch("fetchAllRoutes");
    console.log("Fetched routes:", allRoutes.value);
  } catch (error) {
    console.error("Error fetching route data:", error);
  }
});

const totalPages = computed(() => {
  return Array.isArray(allRoutes.value)
      ? Math.ceil(allRoutes.value.length / itemsPerPage)
      : 0;
});

const changePage = (page) => {
  if (page >= 1 && page <= totalPages.value) {
    currentPage.value = page;
  }
};
</script>

<template>
  <div class="routes-table">
    <h1>Climbing Routes</h1>
    <table v-if="paginatedRoutes.length > 0">
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
      <tr v-for="route in paginatedRoutes" :key="route.id">
        <td>{{ route.route }}</td>
        <td>{{ route.location }}</td>
        <td>{{ route.rating }}</td>
        <td>{{ route.avgStars }}</td>
        <td>{{ route.length }}</td>
      </tr>
      </tbody>
    </table>
    <p v-else>Loading routes...</p>
    <div class="pagination" v-if="totalPages > 1">
      <button @click="changePage(currentPage - 1)" :disabled="currentPage === 1">
        Previous
      </button>
      <span>Page {{ currentPage }} of {{ totalPages }}</span>
      <button @click="changePage(currentPage + 1)" :disabled="currentPage === totalPages">
        Next
      </button>
    </div>
  </div>
</template>

<style scoped>
.routes-table {
  margin: 20px auto;
  max-width: 800px;
}

.routes-table table {
  width: 100%;
  border-collapse: collapse;
}

.routes-table th,
.routes-table td {
  border: 1px solid #ddd;
  padding: 8px;
}

.routes-table th {
  background-color: #f2f2f2;
  text-align: left;
}

.pagination {
  margin-top: 10px;
  display: flex;
  justify-content: space-between;
  align-items: center;
}
</style>

<script setup>
import { computed, onMounted } from 'vue';
import { useStore } from 'vuex'; // Import the useStore hook

const store = useStore(); // Access the Vuex store

// Computed property to get route data from Vuex store
const routeData = computed(() => store.getters.routeData);

// Fetch route data on mounted
onMounted(() => {
  store.dispatch("fetchRouteData", 2); // Replace '1' with your dynamic routeId if needed
});
</script>

<template>
  <div v-if="routeData" class="route-details-box">
    <h1>Climbing Route Details</h1>
    <div>
      <strong>Route:</strong> {{ routeData.route }}
    </div>
    <div>
      <strong>Date:</strong> {{ new Date(routeData.date).toLocaleString() }}
    </div>
    <div>
      <strong>Location:</strong> {{ routeData.location }}
    </div>
    <div>
      <strong>Rating:</strong> {{ routeData.rating }}
    </div>
    <div>
      <strong>Notes:</strong> {{ routeData.notes }}
    </div>
    <div>
      <strong>URL:</strong> <a :href="routeData.url" target="_blank">Visit Route</a>
    </div>
    <div>
      <strong>Pitches:</strong> {{ routeData.pitches }}
    </div>
    <div>
      <strong>Average Stars:</strong> {{ routeData.avgStars }}
    </div>
    <div>
      <strong>Your Stars:</strong> {{ routeData.yourStars }}
    </div>
    <div>
      <strong>Style:</strong> {{ routeData.style }}
    </div>
    <div>
      <strong>Lead Style:</strong> {{ routeData.leadStyle }}
    </div>
    <div>
      <strong>Route Type:</strong> {{ routeData.routeType }}
    </div>
    <div>
      <strong>Your Rating:</strong> {{ routeData.yourRating }}
    </div>
    <div>
      <strong>Length:</strong> {{ routeData.length }} meters
    </div>
    <div>
      <strong>Rating Code:</strong> {{ routeData.ratingCode }}
    </div>
  </div>
  <div v-else>
    <p>Loading...</p>
  </div>
</template>


<style scoped>
/* Add styling for the route details container */
.route-details-box {
  background-color: white; /* Keep the background white */
  border-radius: 8px; /* Slightly rounded corners */
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1), 0 1px 3px rgba(0, 0, 0, 0.06); /* Subtle shadow */
  padding: 16px; /* Add space inside the box */
  margin: 20px auto; /* Center the box with some margin */
  max-width: 600px; /* Limit the width for readability */
  border: 1px solid rgba(0, 0, 0, 0.1); /* Add a very light border */
}

/* Add some spacing between items */
.route-details-box div {
  margin-bottom: 10px; /* Space between lines */
}

/* Style the heading */
.route-details-box h1 {
  font-size: 1.5rem;
  margin-bottom: 16px; /* Add space below the heading */
  text-align: center; /* Center the heading text */
}

/* Style the link */
.route-details-box a {
  color: #007BFF; /* Use a standard blue color */
  text-decoration: none; /* Remove underline */
}

.route-details-box a:hover {
  text-decoration: underline; /* Underline link on hover */
}
</style>
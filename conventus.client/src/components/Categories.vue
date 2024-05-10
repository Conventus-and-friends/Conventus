<script setup lang="ts">
import { ref, onMounted } from "vue";

const categories = ref([]);

onMounted(async () => {
  try {
    const response = await fetch('http://localhost:5020/api/Categories');
    const data = await response.json();
    categories.value = data.sort((a, b) => a.name.localeCompare(b.name));
  } catch (error) {
    console.error('Error fetching categories:', error);
  }
});
</script>

<template>
  <div class="card">
    <ul>
      <li v-for="category in categories" :key="category.id">
        <h3>{{ category.name }}</h3>
        <p>{{ category.description }}</p>
      </li>
    </ul>
  </div>
</template>

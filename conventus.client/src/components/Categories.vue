<script setup lang="ts">
import { ref, onMounted } from "vue";
import DataView from "primevue/dataview";
import Divider from 'primevue/divider';


const categories = ref([
  {
    id: 1,
    name: "syidusuiad"
  },
  {
    id: 2,
    name: "syiduasdasssuiad"
  },
  {
    id: 2,
    name: "syiduasdasssuiad"
  },
  {
    id: 2,
    name: "syiduasdasssuiad"
  }
]);

onMounted(async () => {
  
});


</script>

<template>
  <div class="card">
    <DataView :value="categories" data-key="id">
            <template #list="slotProps">
                <div class="grid grid-nogutter">
                    <div v-for="(item, index) in slotProps.items" :key="index" class="col-12">
                        <div class="flex flex-column sm:flex-row sm:align-items-center p-4 gap-3">
                            <Divider v-if="index > 0" />
                            <div>
                              <h1>{{ item.name }}{{ index }}</h1>
                            </div>
                        </div>
                    </div>
                </div>
            </template>
      </DataView>
  </div>
</template>

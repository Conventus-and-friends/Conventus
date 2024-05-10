<script setup lang="ts">
import { ref, onMounted } from "vue";
import DataView from "primevue/dataview";
import Divider from 'primevue/divider';
import type { Category } from "@/models/category";
import { getCategories } from "@/services/categoryService";
import { isMobile, truncateText } from "@/helpers";

const categories = ref<Category[]>();

onMounted(async () => {
    categories.value = await getCategories();
});

function truncateDescription(text: string): string {
    if (isMobile()) {
      return truncateText(text, 70);
    }
    return truncateText(text, 150);
}

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
                              <h4>{{ item.name }}</h4>
                              <p>{{ truncateDescription(item.description) }}</p>
                            </div>
                        </div>
                    </div>
                </div>
            </template>
      </DataView>
  </div>
</template>

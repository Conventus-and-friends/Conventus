<script setup lang="ts">
import { ref, onMounted } from "vue";
import DataView from "primevue/dataview";
import Divider from 'primevue/divider';
import type { Category } from "@/models/category";
import { getCategories } from "@/services/categoryService";
import { isMobile, truncateText } from "@/helpers";
import { RouterLink } from "vue-router";
import { useRouteParams } from "@vueuse/router";
import { useI18n } from "vue-i18n";

const i18n = useI18n();

const categories = ref<Category[] | null>(null);

onMounted(async () => {
    const values = await getCategories();
    if (Array.isArray(values) && values.length > 0) {
      categories.value = values.sort((a, b) => a.name.localeCompare(b.name));
    }
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
    <DataView v-if="categories" :value="categories" data-key="id">
            <template #list="slotProps">
                <div class="grid grid-nogutter">
                    <div v-for="(item, index) in slotProps.items" :key="index" class="col-12">
                        <div class="flex flex-column sm:flex-row sm:align-items-center p-4 gap-3">
                            <Divider v-if="index > 0" />
                            <div class="hoverbox">
                              <RouterLink style="text-decoration: none; color: inherit;" :to="{ name: 'category', params: { locale: useRouteParams('locale')?.value ??  i18n.locale.value, category: item.id } }" >
                                <h4>{{ item.name }}</h4>
                                <p v-if="item.description">{{ truncateDescription(item.description) }}</p>
                              </RouterLink>
                            </div>
                        </div>
                    </div>
                </div>
            </template>
      </DataView>
  </div>
</template>

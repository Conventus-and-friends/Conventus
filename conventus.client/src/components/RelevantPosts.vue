<script setup lang="ts">
import { ref, onMounted } from "vue";
import DataView from "primevue/dataview";
import Divider from 'primevue/divider';
import type { Post } from "@/models/post";
import { getRelevantPosts } from "@/services/postService";
import { isMobile, removeHtmlFromText, truncateText } from "@/helpers";
import { RouterLink } from "vue-router";
import { useRouteParams } from "@vueuse/router";
import { useI18n } from "vue-i18n";

const i18n = useI18n();

const posts = ref<Post[] | null>();

onMounted(async () => {
    const values = await getRelevantPosts(6);
    if (Array.isArray(values) && values.length > 0) {
      posts.value = values;
    }
});

function formatContent(text: string): string {
    const noHtml = removeHtmlFromText(text)
    if (isMobile()) {
      return truncateText(noHtml, 100)
    }
    return truncateText(noHtml, 250)
}

</script>

<template>
  <div class="card">
    <DataView v-if="posts" :value="posts" data-key="id">
            <template #list="slotProps">
                <div class="grid grid-nogutter">
                    <div v-for="(item, index) in slotProps.items" :key="index" class="col-12">
                        <div class="flex flex-column sm:flex-row sm:align-items-center p-4 gap-3">
                            <Divider v-if="index > 0" />
                            <div class="hoverbox">
                              <RouterLink style="text-decoration: none; color: inherit;" :to="{ name: 'post', params: { locale: useRouteParams('locale')?.value ??  i18n.locale.value, category: item.category, post: item.id } }" >
                                <h4>{{ item.title }}</h4>
                                <p v-if="item.content">{{ formatContent(item.content) }}</p>
                              </RouterLink>
                            </div>
                        </div>
                    </div>
                </div>
            </template>
      </DataView>
  </div>
</template>

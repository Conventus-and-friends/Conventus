<script setup lang="ts">
import { ref } from "vue";
import type { Post } from "@/models/post";
import { getSimilarPosts } from "@/services/postService";
import { isMobile, removeHtmlFromText, truncateText } from "@/helpers";
import DataView from "primevue/dataview";
import Divider from "primevue/divider";
import { RouterLink } from "vue-router";
import { useRouteParams } from "@vueuse/router";
import { useI18n } from "vue-i18n";
import { asyncComputed } from "@vueuse/core";

const i18n = useI18n();


const props = defineProps({
    post: { type: Object as () => Post, required: true }
});

const posts = asyncComputed(
    async () => {
        if (!props.post.id) return null
        const values = await getSimilarPosts(props.post.id, 10);
        if (Array.isArray(values) && values.length > 0) {
            return values
        }
        return null
    },
    null
)

function formatContent(text: string): string {
    const noHtml = removeHtmlFromText(text)
    if (isMobile()) {
      return truncateText(noHtml, 40)
    }
    return truncateText(noHtml, 100)
}
</script>

<template>
    <div class="m-0">
        <DataView v-if="posts" :value="posts" data-key="id">
            <template #list="slotProps">
                <div class="grid grid-nogutter">
                    <div v-for="(item, index) in slotProps.items" :key="index" class="col-12">
                        <div class="flex flex-column sm:flex-row sm:align-items-center">
                            <Divider v-if="index > 0" style="padding-bottom: 1rem;" />
                            <div class="hoverbox">
                              <RouterLink style="text-decoration: none; color: inherit;" :to="{ name: 'post', params: { locale: useRouteParams('locale')?.value ??  i18n.locale.value, category: item.category, post: item.id } }" >
                                <h4>{{ item.title }}</h4>
                                <p v-if="item.content" class="break-word">{{ formatContent(item.content) }}</p>
                              </RouterLink>
                            </div>
                        </div>
                    </div>
                </div>
            </template>
      </DataView>
    </div>
</template>

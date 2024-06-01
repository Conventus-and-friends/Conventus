<script setup lang="ts">
import Card from 'primevue/card';
import Panel from 'primevue/panel';
import ScrollPanel from 'primevue/scrollpanel';
import { useI18n } from 'vue-i18n';
import Comments from '@/components/Comments.vue';
import SimilarPosts from '@/components/SimilarPosts.vue';
import type { Category } from '@/models/category';
import type { Post } from '@/models/post';
import { getPost } from '@/services/postService';
import { asyncComputed, useTitle } from "@vueuse/core";
import { useRouteParams } from '@vueuse/router';
import { computed, onMounted, ref } from 'vue';
import { getCategory } from '@/services/categoryService';
import { useRouter } from 'vue-router';
import { dateAsUtcDate } from '@/helpers';

import DOMPurify from "dompurify";

const router = useRouter();

const i18n = useI18n();
const locale = useRouteParams('locale')?.value as string ??  i18n.locale.value

const title = useTitle();
const router404Args = { name: "404", params: { locale:  locale} }

const categoryIdRaw = useRouteParams("category");
const categoryId = computed(() => {
    if (typeof(categoryIdRaw.value) === "string") {
        return parseInt(categoryIdRaw.value)
    }
    return null
});
const category = asyncComputed(
    async () => {
        if (categoryId.value) {
            const value = await getCategory(categoryId.value);
            if (value) {
                title.value = "Conventus - " + value.name
                return value
            }
            router.push(router404Args)
        }
        console.warn("invalid category id")
        return null
    },
    null
)

const postId = useRouteParams("post");
const post = asyncComputed(
    async () => {
        if (typeof(postId.value) === "string") {
            const value = await getPost(postId.value);
            if (value) {
                title.value = title.value + " - " + value.title
                if (value.category === categoryId.value) {
                    return value
                }
            }
            router.push(router404Args)
        }
        console.warn("invalid post id")
        return null
    },
    null
)

const { d, t } = i18n
</script>

<template>
    <div class="flex-container-overflow top-margin-2">
        <div class="flex-item last-item" v-if="post">
            <Card>
                <template #title>{{ DOMPurify.sanitize(post.title) }}</template>
                <template #subtitle v-if="post.created">{{ d(dateAsUtcDate(post.created), "long") }}</template>
                <template #content>
                    <div v-if="post.content" v-html="DOMPurify.sanitize(post.content)" class="m-0 break-word"></div>
                </template>
            </Card>
            <Comments :post="post"/>
        </div>


        <Panel :header="t('post.similar-posts')" class="flex-item hidden-on-mobile">
            <ScrollPanel style="width: 100%; height: 25rem;">
                <SimilarPosts v-if="post" :post="post"/>
            </ScrollPanel>
        </Panel>
    </div>
</template>

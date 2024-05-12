<script setup lang="ts">
import Card from 'primevue/card';
import Panel from 'primevue/panel';
import { useI18n } from 'vue-i18n';

import type { Category } from '@/models/category';
import type { Post } from '@/models/post';
import { getPost } from '@/services/postService';
import { useRouteParams } from '@vueuse/router';
import { onMounted, ref } from 'vue';
import { getCategory } from '@/services/categoryService';
import { useRouter } from 'vue-router';

import { sanitize } from 'dompurify';

const router = useRouter();

const i18n = useI18n();
const locale = useRouteParams('locale')?.value as string ??  i18n.locale.value

const categoryIdRaw = useRouteParams("category");
const categoryId = ref<number | null>(null);
const category = ref<Category>();

const postIdRaw = useRouteParams("post");
const postId = ref<string | null>(null);
const post = ref<Post>();

onMounted(async () => {
    if (typeof(categoryIdRaw.value) === "string") {
        const id = parseInt(categoryIdRaw.value)
        const value = await getCategory(id);
        if (value) {
            category.value = value
            categoryId.value = id
        } else {
            router.push({ name: "404", params: { locale:  locale} })
        }
    } else {
        console.warn("invalid category id")
    }

    if (typeof(postIdRaw.value) === "string") {
        const value = await getPost(postIdRaw.value);
        if (value) {
            post.value = value
            postId.value = postIdRaw.value
        } else {
            router.push({ name: "404", params: { locale:  locale} })
        }
    } else {
        console.warn("invalid post id")
    }
})

const { t } = i18n
</script>

<template>
    <div class="flex-container-overflow top-margin-2">
        <Card v-if="post" class="flex-item last-item">
            <template #title>{{ sanitize(post.title) }}</template>
            <template #content>
                <div v-if="post.content" v-html="sanitize(post.content)" class="m-0">
                </div>
            </template>
        </Card>

        <Panel header="Header" class="flex-item">
            <p class="m-0">
                Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo
                consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.
            </p>
        </Panel>
    </div>
</template>

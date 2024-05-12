<script setup lang="ts">
import type { Category } from "@/models/category";
import { getCategory } from "@/services/categoryService";
import { onMounted, ref } from "vue";
import { useRouter } from "vue-router";
import { useRouteParams } from "@vueuse/router";
import { useI18n } from "vue-i18n";
import Panel from 'primevue/panel';
import Dialog from 'primevue/dialog';
import Paginator from 'primevue/paginator';
import Button from 'primevue/button';
import { getPostsPageCount, getPosts } from "@/services/postService";

const i18n = useI18n();
const locale = useRouteParams('locale')?.value as string ??  i18n.locale.value

// consts for api requests of categories and posts
const lengthPage = 10;

// router stuff
const router = useRouter();

const categoryId = useRouteParams("category");

const category = ref<Category>();
const visible = ref(false);
const postCount = ref(0);

onMounted(async () => {
    if (typeof(categoryId.value) === "string") {
        const id = parseInt(categoryId.value)
        const value = await getCategory(id);
        if (value) {
            category.value = value
            postCount.value = await getPostsPageCount(id, lengthPage)
        } else {
            router.push({ name: "404", params: { locale:  locale} })
        }
    } else {
        console.warn("invalid category id")
    }
})

// set constants
const { t } = useI18n()
</script>

<template>
    <!-- Information -->
    <div class="margin-infobtn top-margin">
        <h2>{{ category?.name }}</h2>
        <Button label="i" @click="visible = true" />
    </div>

    <Dialog v-model:visible="visible" maximizable modal :header="category?.name" :style="{ width: '50rem' }" :breakpoints="{ '1199px': '75vw', '575px': '90vw' }">
        <p class="m-0">
            {{ category?.description }}
        </p>
    </Dialog>

    <!-- Post panel -->
    <div class="flex-container-overflow">
        <Panel :header="t('category.posts')" class="flex-item last-item">
            <p class="m-0">
                Here are all posts
            </p>
            <Paginator :rows=lengthPage :totalRecords="120" :rowsPerPageOptions="[10, 20, 30]"></Paginator>
        </Panel>

        <Panel :header="t('category.actions')" class="flex-item">
            <p class="m-0">
                - action 1
            </p>
            <p>
                - action 2
            </p>
            <p>
                - action 3
            </p>
        </Panel>
    </div>

</template>

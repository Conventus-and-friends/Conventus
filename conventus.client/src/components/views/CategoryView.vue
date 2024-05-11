<script setup lang="ts">
import type { Category } from "@/models/category";
import { getCategory } from "@/services/categoryService";
import { onMounted, ref } from "vue";
import { useRouter } from "vue-router";
import { useRouteParams } from "@vueuse/router";
import { useI18n } from "vue-i18n";
import Panel from 'primevue/panel';

const i18n = useI18n();
const locale = useRouteParams('locale')?.value as string ??  i18n.locale.value

const router = useRouter();

const categoryId = useRouteParams("category");

const category = ref<Category>()

onMounted(async () => {
    if (typeof(categoryId.value) === "string") {
        const value = await getCategory(parseInt(categoryId.value));
        if (value) {
            category.value = value
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
    <!-- Information (toggable) panel -->
    <Panel :header="category?.name" toggleable class="top-margin-2">
        <p class="m-0">
            {{ category?.description }}
        </p>
    </Panel>

    <!-- Post panel -->
    <div class="flex-container-overflow">
        <Panel :header="t('category.posts')" class="top-margin flex-item last-item">
            <p class="m-0">
                Here are all posts
            </p>
        </Panel>

        <Panel :header="t('category.actions')" class="top-margin flex-item">
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

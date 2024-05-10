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
</script>

<template>
    <Panel :header="category?.name" toggleable class="top-margin">
        <p class="m-0">
            {{ category?.description }}
        </p>
    </Panel>
</template>

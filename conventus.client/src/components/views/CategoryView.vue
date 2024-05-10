<script setup lang="ts">
import type { Category } from "@/models/category";
import { getCategory } from "@/services/categoryService";
import { useRouteParams } from "@vueuse/router";
import { onMounted, ref } from "vue";
const categoryId = useRouteParams("category");

const category = ref<Category>()

onMounted(async () => {
    if (typeof(categoryId) === "string") {
        category.value = await getCategory(parseInt(categoryId));
    } else {
        console.warn("invalid category id")
    }
})
</script>
<template>
    <h1>{{ category?.name }}</h1>
</template>
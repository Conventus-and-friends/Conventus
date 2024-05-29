<script setup lang="ts">
// lang imports
import { useI18n } from "vue-i18n";

// primevue imports
import Panel from 'primevue/panel';

// local imports
import Categories from "@/components/Categories.vue";
import RelevancePost from "@/components/RelevancePost.vue";

// set constants
const { t } = useI18n()
</script>

<template>
    <div class="top-margin-2">
        <h2>{{ t('startpage.start') }}</h2>
    </div>

    <div class="flex-container">
        <Panel class="flex-item-normal last-item" :header="t('startpage.categories')">
            <Categories/>
        </Panel>

        <Panel class="flex-item-normal" :header="t('startpage.relevant')">
            <RelevancePost/>
        </Panel>
    </div>
</template>

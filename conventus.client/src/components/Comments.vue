<script setup lang="ts">
import { useI18n } from 'vue-i18n';
import Card from 'primevue/card';
import type { Post } from '@/models/post';
import { onMounted, ref } from 'vue';
import { getComments, getCommentsCount } from '@/services/commentService';
import { asyncComputed } from '@vueuse/core';
import DataView from 'primevue/dataview';
import Paginator from 'primevue/paginator';
import DOMPurify from 'dompurify';

const props = defineProps({
    post: { type: Object as () => Post, required: true }
});

const i18n = useI18n();
const { t } = i18n

// paginator values
const currentPage = ref(0);
const itemsPerPage = ref(10);

// api request
const commentCount = ref(0)

onMounted(async () => {
    if (props.post) {
        commentCount.value = await getCommentsCount(props.post.id ?? "")
    }
})

const comments = asyncComputed(
    async () => {
        if (props.post) {
            return await getComments(props.post.id ?? "", (currentPage.value / itemsPerPage.value) + 1, itemsPerPage.value);
        }
        return null;
    },
    null
)
</script>

<template>
    <div class="i3-4">
        <h3>{{ t('post.comments') }}</h3>
        <DataView v-if="comments" :value="comments" dataKey="id">
            <template #list="slotProps">
                <div class="grid grid-nogutter">
                    <div v-for="(item, index) in slotProps.items" :key="index" class="col-12">
                        <div class="flex flex-column sm:flex-row sm:align-items-center p-4 gap-3">
                            <Divider v-if="index > 0" />
                            <Card>
                                <template #subtitle>Nutzername - vor 10 Minuten</template>
                                <template #content>{{ DOMPurify.sanitize(item.content) }}</template>
                            </Card>
                        </div>
                    </div>
                </div>
            </template>
        </DataView>
        <Paginator v-model:first="currentPage" v-model:rows="itemsPerPage" :totalRecords="commentCount" :rowsPerPageOptions="[10, 20, 30, 40, 50]" class="top-margin"></Paginator>
    </div>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n';
import Card from 'primevue/card';
import type { Post } from '@/models/post';
import { onMounted, ref } from 'vue';
import { getComments } from '@/services/commentService';
import { asyncComputed } from '@vueuse/core';

const props = defineProps({
    post: { type: Object as () => Post, required: true }
});

const i18n = useI18n();
const { t } = i18n

// api request
const comments = asyncComputed(
    async () => {
        if (props.post) {
            return await getComments(props.post.id ?? "", 1, 10);
        }
    }
)
</script>

<template>
    <div class="i3-4">
        <h3>{{ t('post.comments') }}</h3>
        <Card>
            <template #subtitle>Nutzername - vor 10 Minuten</template>
            <template #content>Bla Bla Bla answer</template>
        </Card>
    </div>
</template>

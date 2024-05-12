<script setup lang="ts">
import { useI18n } from 'vue-i18n';
import { ref, defineProps, computed } from 'vue';

import Button from 'primevue/button';
import Editor from 'primevue/editor';
import InputText from 'primevue/inputtext';

import type { Category } from '@/models/category';

const { t } = useI18n();

const props = defineProps({
    category: { type: Object as () => Category, required: true }
});

const content = ref('');
const title = ref('');

const underlineText = computed(() =>  t('util.underline'))
const boldText = computed(() =>  t('util.bold'))
const italicText = computed(() =>  t('util.italic'))
</script>
<template>
    <div>
        <InputText v-model="title" :placeholder="t('util.title')" />
        <Editor v-model="content" editorStyle="height: 400px" class="top-margin">
            <template v-slot:toolbar>
                <span class="ql-formats">
                    <Button v-tooltip.top="boldText" class="ql-bold"></Button>
                    <Button v-tooltip.top="italicText" class="ql-italic"></Button>
                    <Button v-tooltip.top="underlineText" class="ql-underline"></Button>
                </span>
            </template>
        </Editor>
        <div class="flex justify-content-end gap-2">
            <Button type="button" :label="t('util.cancel')" severity="secondary" @click="$emit('cancelled')" class="top-margin last-item"></Button>
            <Button type="button" :label="t('util.post')" @click="$emit('posted')" class="top-margin"></Button>
        </div>
    </div>
</template>, computed
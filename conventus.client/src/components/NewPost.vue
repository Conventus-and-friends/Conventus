<script setup lang="ts">
import { useI18n } from 'vue-i18n';
import { ref } from 'vue';

import Button from 'primevue/button';
import Editor from 'primevue/editor';
import InputText from 'primevue/inputtext';

const { t } = useI18n();

const content = ref('');
const title = ref('');
</script>
<template>
    <InputText v-model="title" :placeholder="t('util.title')" />
    <Editor v-model="content" editorStyle="height: 400px" class="top-margin">
        <template v-slot:toolbar>
            <span class="ql-formats">
                <button v-tooltip.bottom="'Bold'" class="ql-bold"></button>
                <button v-tooltip.bottom="'Italic'" class="ql-italic"></button>
                <button v-tooltip.bottom="'Underline'" class="ql-underline"></button>
            </span>
        </template>
    </Editor>
    <div class="flex justify-content-end gap-2">
        <Button type="button" :label="t('util.cancel')" severity="secondary" @click="$emit('cancelled')" class="top-margin last-item"></Button>
        <Button type="button" :label="t('util.post')" @click="$emit('posted')" class="top-margin"></Button>
    </div>
</template>
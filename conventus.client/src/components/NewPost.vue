<script setup lang="ts">
import { useI18n } from 'vue-i18n';
import { ref, computed } from 'vue';

import Button from 'primevue/button';
import Editor from 'primevue/editor';
import InputText from 'primevue/inputtext';

import type { Category } from '@/models/category';
import { newPost } from '@/services/postService';
import type { Post } from '@/models/post';

const { t } = useI18n();

const props = defineProps({
    category: { type: Object as () => Category, required: true }
});

const content = ref('');
const title = ref('');

const underlineText = computed(() =>  t('util.underline'))
const boldText = computed(() =>  t('util.bold'))
const italicText = computed(() =>  t('util.italic'))

// methods
function submitPost(): Promise<Post | null> {
    const category = props.category
    const post: Post = {
        title: title.value,
        content: content.value.trim().length > 0 ? content.value : undefined,
        category: category.id ?? 0,
        id: undefined
    }
    return newPost(post);
}

function lengthInfo() {
    const length = title.value.trim().length
    const titleLengthText = document.getElementById('titleLengthText');
    if (titleLengthText) {
        if (length !== undefined && length > 50) {
            document.getElementById('postButton')?.setAttribute('disabled', 'disabled');
            titleLengthText.setAttribute('style', 'margin-top: 5px; color: red; font-size: small;');
        } else {
            document.getElementById('postButton')?.removeAttribute('disabled');
            titleLengthText.removeAttribute('style');
            titleLengthText.setAttribute('style', 'margin-top: 5px; font-size: small;');
            titleLengthText.textContent = `${length}/50`;
        }
    }
}
</script>

<template>
    <div>
        <div style="display: flex; flex-direction: column;">
            <InputText @input="lengthInfo()" v-model="title" :placeholder="t('util.title')" />
            <p id="titleLengthText" style="margin-top: 5px; font-size: small;">0/50</p>
        </div>
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
            <Button type="button" :label="t('util.post')" @click="submitPost().then(post => $emit('posted', post))" class="top-margin" id="postButton"></Button>
        </div>
    </div>
</template>, computed

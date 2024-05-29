<script setup lang="ts">
import { useI18n } from 'vue-i18n';
import Card from 'primevue/card';
import type { Post } from '@/models/post';
import { computed, onMounted, ref } from 'vue';
import { getComments, getCommentsCount, newComment } from '@/services/commentService';
import { asyncComputed } from '@vueuse/core';
import type { Comment } from '@/models/comment';
import DataView from 'primevue/dataview';
import Paginator from 'primevue/paginator';
import Accordion from 'primevue/accordion';
import AccordionTab from 'primevue/accordiontab';
import Editor from 'primevue/editor';
import Button from 'primevue/button';
import DOMPurify from 'dompurify';
import { useToast } from 'primevue/usetoast';
import { useLocaleTimeAgo } from '@/i18n';
import { utcAsLocalDate } from '@/helpers';

const toasts = useToast()

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

async function getCommentsFromService(): Promise<Comment[] | null> {
    return await getComments(props.post.id ?? "", (currentPage.value / itemsPerPage.value) + 1, itemsPerPage.value)
}

const comments = asyncComputed(
    async () => {
        if (props.post) {
            const comments = await getCommentsFromService()
            if (Array.isArray(comments) && comments.length > 0) {
                return comments
            }
        }
        return null;
    },
    null
)

const content = ref('');
const submitDisabled = ref(true);

const underlineText = computed(() =>  t('util.underline'))
const boldText = computed(() =>  t('util.bold'))
const italicText = computed(() =>  t('util.italic'))

function checkLength() {
    submitDisabled.value = content.value.trim().length <= 0
}

const activeIndex = ref<number>(1)

function abort() {
    activeIndex.value = activeIndex.value + 1 // why does this work? I hate this.
    content.value = ''
}

function submitComment() {
    const comment: Comment | null = {
        id: undefined,
        content: content.value.trim(),
        post: props.post.id ?? "",
        created: undefined
    }
    newComment(comment).then(() => {
        if (props.post) {
            getCommentsCount(props.post.id ?? "").then((count) => commentCount.value = count)
            currentPage.value = 0
            getCommentsFromService().then((c) => comments.value = c)

            toasts.add({ severity: 'success', summary: t("comment.comment-added"), detail: t("comment.comment-added-description"), life: 3500 })
            abort() // close the comment editor
        }
    })
}
</script>

<template>
    <div class="i3-4">
        <h3>{{ t('comment.comments') }}</h3>
        <Accordion class="top-margin" v-bind:active-index="activeIndex">
            <AccordionTab :header="t('comment.new-comment')">
                <Editor v-on:text-change="checkLength()" v-model="content" editorStyle="height: 200px" class="top-margin">
                    <template v-slot:toolbar>
                        <span class="ql-formats">
                            <Button v-tooltip.top="boldText" class="ql-bold"></Button>
                            <Button v-tooltip.top="italicText" class="ql-italic"></Button>
                            <Button v-tooltip.top="underlineText" class="ql-underline"></Button>
                        </span>
                    </template>
                </Editor>
                <div>
                    <div class="top-margin align-right">
                        <Button type="button" :label="t('util.cancel')" severity="secondary" @click="abort()" text rounded size="small" style="margin-right: 0.3rem;"></Button>
                        <Button type="button" :label="t('comment.comment')" @click="submitComment()" :disabled="submitDisabled" rounded size="small"></Button>
                    </div>
                </div>
            </AccordionTab>
        </Accordion>
        <DataView v-if="comments" :value="comments" dataKey="id">
            <template #list="slotProps">
                <div class="grid grid-nogutter">
                    <div v-for="(item, index) in slotProps.items" :key="index" class="col-12">
                        <div class="flex flex-column sm:flex-row sm:align-items-center p-4 gap-3">
                            <Card class="top-margin">
                                <template #subtitle>Username - {{ useLocaleTimeAgo(utcAsLocalDate(item.created)).value }}</template>
                                <template #content><div class="p-no-margins" v-html="DOMPurify.sanitize(item.content)"></div></template>
                            </Card>
                        </div>
                    </div>
                </div>
            </template>
        </DataView>
        <Paginator v-model:first="currentPage" v-model:rows="itemsPerPage" :totalRecords="commentCount" :rowsPerPageOptions="[10, 20, 30, 40, 50]" class="top-margin"></Paginator>
    </div>
</template>

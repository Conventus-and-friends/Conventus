<script setup lang="ts">
import { searchPosts } from '@/services/postService';
import { asyncComputed } from '@vueuse/core';
import { removeHtmlFromText, truncateText } from '@/helpers';
import { useRouteParams } from '@vueuse/router';
import { useI18n } from 'vue-i18n';
import DataView from 'primevue/dataview';
import { RouterLink } from 'vue-router';
import Divider from 'primevue/divider';

const searchText = defineModel<string | undefined>("searchText", { required: true });
const emit = defineEmits(['close']);
const i18n = useI18n();
const { t } = i18n

const searchResults = asyncComputed(
    async () => {
        if (searchText.value) {
            const values = await searchPosts(searchText.value, 5)
            if (Array.isArray(values) && values.length > 0) {
                return values
            }
        }
        return null
    },
    null
)

function formatContent(text: string): string {
    const noHtml = removeHtmlFromText(text)
    return truncateText(noHtml, 30)
}
</script>

<template>
    <div>
        <DataView v-if="searchResults" :value="searchResults" data-key="id">
                <template #list="slotProps">
                    <div class="grid grid-nogutter">
                        <div v-for="(item, index) in slotProps.items" :key="index" class="col-12">
                            <div class="flex flex-column sm:flex-row sm:align-items-center p-4 gap-3">
                                <Divider v-if="index > 0" />
                                <div class="hoverbox">
                                <RouterLink @click.native="emit('close')" style="text-decoration: none; color: inherit;" :to="{ name: 'post', params: { locale: useRouteParams('locale')?.value ??  i18n.locale.value, category: item.category, post: item.id } }" >
                                    <h4 class="break-word">{{ truncateText(item.title, 30) }}</h4>
                                    <p v-if="item.content" class="break-word">{{ formatContent(item.content) }}</p>
                                </RouterLink>
                                </div>
                            </div>
                        </div>
                    </div>
                </template>
        </DataView>
        <p v-else>{{ t('navbar.no-results') }}</p>
    </div>
</template>

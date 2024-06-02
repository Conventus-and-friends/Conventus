<script setup lang="ts">
import { searchPosts } from '@/services/postService';
import { asyncComputed, useThrottleFn } from '@vueuse/core';
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

const debouncedSearch = useThrottleFn((query: string) => searchPosts(query, 5), 550)

const searchResults = asyncComputed(
    async () => {
        if (searchText.value) {
            const values = await debouncedSearch(searchText.value)
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
                            <div class="flex flex-column sm:flex-row sm:align-items-center">
                                <Divider v-if="index > 0" style="padding-bottom: 1rem;" />
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

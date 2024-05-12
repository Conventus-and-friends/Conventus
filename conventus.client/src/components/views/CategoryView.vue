<script setup lang="ts">
import type { Category } from "@/models/category";
import { getCategory } from "@/services/categoryService";
import { onMounted, ref } from "vue";
import { useRouter } from "vue-router";
import { useRouteParams } from "@vueuse/router";
import { useI18n } from "vue-i18n";
import Panel from 'primevue/panel';
import Dialog from 'primevue/dialog';
import Paginator from 'primevue/paginator';
import Button from 'primevue/button';
import DataView from "primevue/dataview";
import { getPostsCount, getPosts } from "@/services/postService";
import { asyncComputed } from "@vueuse/core";
import { isMobile, truncateText } from "@/helpers";

const i18n = useI18n();
const locale = useRouteParams('locale')?.value as string ??  i18n.locale.value

// router stuff
const router = useRouter();

const categoryIdRaw = useRouteParams("category");
const categoryId = ref<number | null>(null);

const category = ref<Category>();
const visible = ref(false);
const postCount = ref(0);

// paginator values
const currentPage = ref(1);
const itemsPerPage = ref(10);

onMounted(async () => {
    if (typeof(categoryIdRaw.value) === "string") {
        const id = parseInt(categoryIdRaw.value)
        const value = await getCategory(id);
        if (value) {
            category.value = value
            postCount.value = await getPostsCount(id)
            categoryId.value = id
        } else {
            router.push({ name: "404", params: { locale:  locale} })
        }
    } else {
        console.warn("invalid category id")
    }
})

const posts = asyncComputed(
    async () => {
        if (categoryId.value) {
            return await getPosts(categoryId.value, currentPage.value, itemsPerPage.value)
        }
        return null;
    },
    null
)

// set constants
const { t } = useI18n()

// functions
function truncateContent(text: string): string {
    if (isMobile()) {
      return truncateText(text, 100);
    }
    return truncateText(text, 250);
}
</script>

<template>
    <!-- Information -->
    <div class="margin-infobtn top-margin">
        <h2>{{ category?.name }}</h2>
        <Button label="i" @click="visible = true" />
    </div>

    <Dialog v-model:visible="visible" maximizable modal :header="category?.name" :style="{ width: '50rem' }" :breakpoints="{ '1199px': '75vw', '575px': '90vw' }">
        <p class="m-0">
            {{ category?.description }}
        </p>
    </Dialog>

    <!-- Post panel -->
    <div class="flex-container-overflow">
        <Panel :header="t('category.posts')" class="flex-item last-item">
            <DataView v-if="posts" :value="posts" dataKey="id">
                <template #list="slotProps">
                    <div class="grid grid-nogutter">
                        <div v-for="(item, index) in slotProps.items" :key="index" class="col-12">
                            <div class="flex flex-column sm:flex-row sm:align-items-center p-4 gap-3">
                                <Divider v-if="index > 0" />
                                <div class="hoverbox">
                                    <h3>{{ item.title }}</h3>
                                    <p v-if="item.content" class="m-0">
                                        {{ truncateContent(item.content) }}
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>
                </template>
            </DataView>
            <Paginator v-model:first="currentPage" v-model:rows="itemsPerPage" :totalRecords="postCount" :rowsPerPageOptions="[10, 20, 30, 40, 50]" class="top-margin-2"></Paginator>
        </Panel>

        <Panel :header="t('category.actions')" class="flex-item">
            <p class="m-0">
                - action 1
            </p>
            <p>
                - action 2
            </p>
            <p>
                - action 3
            </p>
        </Panel>
    </div>

</template>

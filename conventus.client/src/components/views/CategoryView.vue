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
import Divider from "primevue/divider";
import DataView from "primevue/dataview";
import Dropdown from 'primevue/dropdown';
import Editor from 'primevue/editor';
import InputText from "primevue/inputtext";
import { getPostsCount, getPosts } from "@/services/postService";
import { asyncComputed } from "@vueuse/core";
import { isMobile, truncateText } from "@/helpers";
import { RouterLink } from "vue-router";

const i18n = useI18n();
const locale = useRouteParams('locale')?.value as string ??  i18n.locale.value

// router stuff
const router = useRouter();

const categoryIdRaw = useRouteParams("category");
const categoryId = ref<number | null>(null);

const category = ref<Category>();
const visible = ref(false);
const visibleCreator = ref(false);
const postCount = ref(0);
const value = ref('');
const title = ref('');

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

// dropdown values
const selectedSorting = ref();
const sortingOptions = ref([
    { name: 'Name (A-Z)' },
    { name: 'Newest' },
    { name: 'Oldest' },
    { name: 'Uncommented' }
]);
</script>

<template>
    <!-- Information -->
    <div class="margin-infobtn top-margin">
        <h2>{{ category?.name }}</h2>
        <Button label="i" @click="visible = true" />
    </div>

    <!-- Info Popup -->
    <Dialog v-model:visible="visible" maximizable modal :header="category?.name" :style="{ width: '50rem' }" :breakpoints="{ '1199px': '75vw', '575px': '90vw' }">
        <p class="m-0">
            {{ category?.description }}
        </p>
    </Dialog>

    <!-- Create new post dialog -->
    <Dialog v-model:visible="visibleCreator" modal :header="t('category.start-discuss')" :style="{ width: '55rem' }">
        <InputText v-model="title" :placeholder="t('util.title')" />
        <Editor v-model="value" editorStyle="height: 400px" class="top-margin">
            <template v-slot:toolbar>
                <span class="ql-formats">
                    <button v-tooltip.bottom="'Bold'" class="ql-bold"></button>
                    <button v-tooltip.bottom="'Italic'" class="ql-italic"></button>
                    <button v-tooltip.bottom="'Underline'" class="ql-underline"></button>
                </span>
            </template>
        </Editor>
        <div class="flex justify-content-end gap-2">
            <Button type="button" :label="t('util.cancel')" severity="secondary" @click="visibleCreator = false" class="top-margin last-item"></Button>
            <Button type="button" :label="t('util.post')" @click="visibleCreator = false" class="top-margin"></Button>
        </div>
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
                                <RouterLink style="text-decoration: none; color: inherit;" :to="{ name: '404', params: { locale:  locale} }">
                                    <div class="hoverbox">
                                        <h3>{{ item.title }}</h3>
                                        <p v-if="item.content" class="m-0">
                                            {{ truncateContent(item.content) }}
                                        </p>
                                    </div>
                                </RouterLink>
                            </div>
                        </div>
                    </div>
                </template>
            </DataView>
            <Paginator v-model:first="currentPage" v-model:rows="itemsPerPage" :totalRecords="postCount" :rowsPerPageOptions="[10, 20, 30, 40, 50]" class="top-margin-2"></Paginator>
        </Panel>

        <Panel :header="t('category.actions')" class="flex-item">
            <Button :label="t('category.start-discuss')" @click="visibleCreator = true" />
            <div class="top-margin">
                <Dropdown v-model="selectedSorting" :options="sortingOptions" optionLabel="name" placeholder="Sort" class="w-full md:w-14rem" />
            </div>
        </Panel>
    </div>
</template>

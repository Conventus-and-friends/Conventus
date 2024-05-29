<script setup lang="ts">
import type { Category } from "@/models/category";
import { getCategory } from "@/services/categoryService";
import { onMounted, ref } from "vue";
import { useRouter } from "vue-router";
import { useRouteParams } from "@vueuse/router";
import { useI18n } from "vue-i18n";
import { useTitle } from "@vueuse/core";
import Panel from 'primevue/panel';
import Dialog from 'primevue/dialog';
import Paginator from 'primevue/paginator';
import Button from 'primevue/button';
import Divider from "primevue/divider";
import DataView from "primevue/dataview";
import Dropdown from 'primevue/dropdown';
import NewPost from "@/components/NewPost.vue";
import { getPostsCount, getPosts } from "@/services/postService";
import { asyncComputed } from "@vueuse/core";
import { isMobile, removeHtmlEntities, truncateText } from "@/helpers";
import { RouterLink } from "vue-router";
import DOMPurify from "dompurify";
import type { Post } from "@/models/post";

const i18n = useI18n();
const locale = useRouteParams('locale')?.value as string ??  i18n.locale.value

// router stuff
const router = useRouter();

const categoryIdRaw = useRouteParams("category");
const categoryId = ref<number | null>(null);

const category = ref<Category>();
const informationVisible = ref(false);
const postCreatorVisible = ref(false);
const postCount = ref(0);

// paginator values
const currentPage = ref(0);
const itemsPerPage = ref(10);

onMounted(async () => {
    if (typeof(categoryIdRaw.value) === "string") {
        const id = parseInt(categoryIdRaw.value)
        const value = await getCategory(id);
        if (value) {
            category.value = value
            postCount.value = await getPostsCount(id)
            categoryId.value = id

            // set title
            useTitle("Conventus - " + value.name)
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
            const posts = await getPosts(categoryId.value, (currentPage.value / itemsPerPage.value) + 1, itemsPerPage.value)
            if (Array.isArray(posts) && posts.length > 0) {
                return posts
            }
        }
        return null;
    },
    null
)

// set constants
const { t } = useI18n()

// functions
function formatContent(text: string): string {
    const noTags = DOMPurify.sanitize(text,
        {
            ALLOWED_TAGS: [],
            ALLOWED_ATTR: [],
            KEEP_CONTENT: true
        }
    )
    const noHtml = removeHtmlEntities(noTags)
    if (isMobile()) {
      return truncateText(noHtml, 100);
    }
    return truncateText(noHtml, 250);
}

// dropdown values
const selectedSorting = ref();
const sortingOptions = ref([
    { name: 'Name (A-Z)' },
    { name: 'Newest' },
    { name: 'Oldest' },
    { name: 'Uncommented' }
]);

function newPost(post: Post | null) {
    postCreatorVisible.value = false;
    router.push({ name: "post", params: { locale: locale, category: category?.value?.id ,post: post?.id } })
}
</script>

<template>
    <!-- Information -->
    <div class="margin-infobtn top-margin">
        <h2>{{ category?.name }}</h2>
        <Button label="i" @click="informationVisible = true" />
    </div>

    <!-- Info Popup -->
    <Dialog v-model:visible="informationVisible" modal :header="category?.name" :style="{ width: '50rem' }" :breakpoints="{ '1199px': '75vw', '575px': '90vw' }">
        <p class="m-0">
            {{ category?.description }}
        </p>
    </Dialog>

    <!-- Create new post dialog -->
    <Dialog v-model:visible="postCreatorVisible" maximizable modal :header="t('category.new-post')" :style="{ width: '55rem' }">
        <NewPost v-if="category" :category="category" @cancelled="postCreatorVisible = false" @posted="newPost"></NewPost>
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
                                <RouterLink v-if="category" style="text-decoration: none; color: inherit;" :to="{ name: 'post', params: { locale:  locale, category: category.id, post: item.id} }">
                                    <div class="hoverbox">
                                        <h3>{{ item.title }}</h3>
                                        <p v-if="item.content" class="m-0">
                                            {{ formatContent(item.content) }}
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

        <!-- Actions panel -->
        <Panel :header="t('category.actions')" class="flex-item">
            <div class="splitscreen">
                <div>
                    <Button :label="t('category.new-post')" @click="postCreatorVisible = true" />
                </div>
                <div class="padding-left">
                    <Dropdown v-model="selectedSorting" :options="sortingOptions" optionLabel="name" placeholder="Sort" class="w-full md:w-14rem" />
                </div>
            </div>
        </Panel>
    </div>
</template>

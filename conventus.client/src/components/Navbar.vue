<script setup lang="ts">
import { ref, type Ref } from "vue";
import Menubar from 'primevue/menubar';
import Badge from "primevue/badge";
import InputText from "primevue/inputtext";
import { useI18n } from "vue-i18n";
import { computed } from "vue";

import { isMobile } from "@/helpers";

const i18n = useI18n()
const { t } = i18n
const locale = computed(() => i18n.locale.value)

const homeText = computed(() =>  t('navbar.home'))
const aboutText = computed(() =>  t('navbar.about'))

const items = ref([
    {
        label: homeText,
        route: { name: 'home', params: { locale: locale} }
    },
    {
        label: aboutText,
        route: { name: 'about', params: { locale: locale} }
    }
]);
</script>

<template>
    <div class="card">
        <Menubar :model="items">
            <template #start>
                <RouterLink :to="{ name: 'home', params: { locale: locale } }">
                    <img v-if="!isMobile()" height="40" src="@/assets/Conventus-Text.svg" class="h-2rem navbar-first-item">
                    <img v-else height="40" src="@/assets/Conventus.svg" class="h-2rem navbar-first-item">
                </RouterLink>
            </template>
            <template #item="{ item, props, hasSubmenu, root }">
                <RouterLink v-if="item.route":to="item.route" style="text-decoration: none; color: inherit;">
                    <a v-ripple class="flex align-items-center" v-bind="props.action">
                        <span :class="item.icon" />
                        <span class="ml-2">{{ item.label }}</span>
                        <Badge v-if="item.badge" :class="{ 'ml-auto': !root, 'ml-2': root }" :value="item.badge" />
                        <span v-if="item.shortcut" class="ml-auto border-1 surface-border border-round surface-100 text-xs p-1">{{ item.shortcut }}</span>
                        <i v-if="hasSubmenu" :class="['pi pi-angle-down', { 'pi-angle-down ml-2': root, 'pi-angle-right ml-auto': !root }]"></i>
                    </a>
                </RouterLink>
                <a v-else v-ripple class="flex align-items-center" v-bind="props.action">
                    <span :class="item.icon" />
                    <span class="ml-2">{{ item.label }}</span>
                    <Badge v-if="item.badge" :class="{ 'ml-auto': !root, 'ml-2': root }" :value="item.badge" />
                    <span v-if="item.shortcut" class="ml-auto border-1 surface-border border-round surface-100 text-xs p-1">{{ item.shortcut }}</span>
                    <i v-if="hasSubmenu" :class="['pi pi-angle-down', { 'pi-angle-down ml-2': root, 'pi-angle-right ml-auto': !root }]"></i>
                </a>
            </template>
            <template #end>
                <div class="flex align-items-center gap-2">
                    <InputText :placeholder="t('navbar.search')" type="text" class="w-8rem sm:w-auto" />
                </div>
            </template>
        </Menubar>
    </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import Menubar from 'primevue/menubar';
import Avatar from "primevue/avatar";
import Badge from "primevue/badge";
import InputText from "primevue/inputtext";

const items = ref([
    /*
    {
        label: 'About',
        icon: 'pi pi-star'
    }
    */
]);
</script>


<template>
    <div class="card">
        <Menubar :model="items">
            <template #start>
                <RouterLink :to="{ name: 'home', params: { locale: $i18n.locale }}">
                    <img v-if="!isMobile()" height="40" src="/src/assets/Conventus-Text.svg" class="h-2rem">
                    <img v-else height="40" src="/src/assets/Conventus.svg" class="h-2rem">
                </RouterLink>
            </template>
            <template #item="{ item, props, hasSubmenu, root }">
                <a v-ripple class="flex align-items-center" v-bind="props.action">
                    <span :class="item.icon" />
                    <span class="ml-2">{{ item.label }}</span>
                    <Badge v-if="item.badge" :class="{ 'ml-auto': !root, 'ml-2': root }" :value="item.badge" />
                    <span v-if="item.shortcut" class="ml-auto border-1 surface-border border-round surface-100 text-xs p-1">{{ item.shortcut }}</span>
                    <i v-if="hasSubmenu" :class="['pi pi-angle-down', { 'pi-angle-down ml-2': root, 'pi-angle-right ml-auto': !root }]"></i>
                </a>
            </template>
            <template #end>
                <div class="flex align-items-center gap-2">
                    <InputText placeholder="Search" type="text" class="w-8rem sm:w-auto" />
                    <Avatar image="https://primefaces.org/cdn/primevue/images/avatar/amyelsner.png" shape="circle" />
                </div>
            </template>
        </Menubar>
    </div>
</template>
<script setup lang="ts">
import { useI18n } from "vue-i18n";
import { useTitle } from "@vueuse/core";

const i18n = useI18n();
const { t } = i18n

// set custom title
useTitle("Conventus - " + t('navbar.about'))
</script>

<template>
    <div class="top-margin-2">
        <h2>{{ t('navbar.about') }}</h2>
    </div>
</template>

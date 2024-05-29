<script setup lang="ts">
import { useI18n } from "vue-i18n";

const i18n = useI18n();
const { t } = i18n
</script>

<template>
    <div class="top-margin-2">
        <h2>{{ t('navbar.about') }}</h2>
    </div>
</template>

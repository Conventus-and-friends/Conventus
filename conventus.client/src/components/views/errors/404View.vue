<script setup lang="ts">
import { useI18n } from "vue-i18n";

const { t } = useI18n()
</script>
<template>
    <div>
        <h1>404</h1>
        <p>{{ t('errors.404') }}</p>
    </div>
</template>

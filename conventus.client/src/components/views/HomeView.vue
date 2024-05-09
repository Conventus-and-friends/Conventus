<script setup lang="ts">
import { useI18n } from "vue-i18n";

const { t } = useI18n()
</script>

<template>
    <div>
        <h2>{{ t('startpage.start') }}</h2>
    </div>
</template>

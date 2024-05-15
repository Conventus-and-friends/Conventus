<script setup lang="ts">
import { useI18n } from 'vue-i18n';
import Card from 'primevue/card';

const i18n = useI18n();
const { t } = i18n
</script>

<template>
    <div class="i3-4">
        <h3>{{ t('util.answers') }}</h3>
        <Card>
            <template #subtitle>Nutzername - vor 10 Minuten</template>
            <template #content>Bla Bla Bla answer</template>
        </Card>
    </div>
</template>

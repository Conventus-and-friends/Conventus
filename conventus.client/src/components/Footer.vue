<script setup lang="ts">
import Divider from 'primevue/divider';
import { useI18n } from 'vue-i18n';
import { ref } from 'vue';

const i18n = useI18n();
const { t } = i18n
const date = ref(new Date().getFullYear());
</script>

<template>
    <Divider class="top-margin-5"/>
    <div class="links-footer">
        <a href="https://github.com/Conventus-and-friends/Conventus" class="footer-link" target="_blank">GitHub</a>
    </div>
    <div class="footer">
        <img src="@/assets/Conventus.svg" alt="conventus" height="40" class="footer-image">
        <p class="footer-text">{{ t('footer.copyright', [date]) }}</p>
    </div>
</template>

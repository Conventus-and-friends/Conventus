<script setup lang="ts">
import Home from './components/Home.vue'
</script>

<template>
    <header>
        <img alt="Logo" class="logo" src="./assets/Conventus.svg" width="125" height="125" />

        <div class="wrapper">
            <Home/>
        </div>
    </header>
</template>

<style scoped>
header {
    line-height: 1.5;
}

.logo {
    display: block;
    margin: 0 auto 2rem;
}

@media (min-width: 1024px) {
    header {
        display: flex;
        place-items: center;
        padding-right: calc(var(--section-gap) / 2);
    }

    .logo {
        margin: 0 2rem 0 0;
    }

    header .wrapper {
        display: flex;
        place-items: flex-start;
        flex-wrap: wrap;
    }
}
</style>

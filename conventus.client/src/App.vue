<script setup lang="ts">
<<<<<<< HEAD
import Home from './components/Home.vue'
=======
    import HelloWorld from './components/HelloWorld.vue'
    import TheWelcome from './components/TheWelcome.vue'
>>>>>>> 834b96c9f55dc4675bb26d9fcaf1dcc5b8674191
</script>

<template>
    <header>
<<<<<<< HEAD
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
=======
        <img alt="Vue logo" class="logo" src="./assets/logo.svg" width="125" height="125" />

        <div class="wrapper">
            <HelloWorld msg="You did it!" />
        </div>
    </header>

    <main>
        <TheWelcome />
    </main>
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
>>>>>>> 834b96c9f55dc4675bb26d9fcaf1dcc5b8674191
</style>

import '@/assets/main.css'
import { useDark } from '@vueuse/core'

if (!useDark().value) {
    import('primevue/resources/themes/lara-light-teal/theme.css')
} else {
    import('primevue/resources/themes/lara-dark-teal/theme.css')
}
import 'primeicons/primeicons.css'

// vue imports
import { createApp } from 'vue'
import App from '@/App.vue'

// primevue and i18n imports
import PrimeVue from 'primevue/config'
import Ripple from 'primevue/ripple'
import ToastService from 'primevue/toastservice'
import { setupI18n } from '@/i18n'
import { setupRouter } from '@/router'

// create vue application
const app = createApp(App)

// Add PrimeVue
app.use(ToastService)
app.directive('ripple', Ripple)
app.use(PrimeVue, { ripple: true })

const i18n = setupI18n()
app.use(i18n)

// Homeview and router
const router = setupRouter(i18n)
app.use(router)

// tooltips
import FloatingVue from 'floating-vue'
import 'floating-vue/dist/style.css'

app.use(FloatingVue)

app.mount('#app')

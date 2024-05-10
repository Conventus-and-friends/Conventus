import '@/assets/main.css'
import 'primevue/resources/themes/lara-light-teal/theme.css'
// import 'primevue/resources/themes/lara-dark-teal/theme.css'

// vue imports
import { createApp } from 'vue'
import App from './App.vue'

// primevue and i18n imports
import PrimeVue from 'primevue/config'
import Ripple from 'primevue/ripple'
import { setupI18n } from '@/i18n'
import { setupRouter } from '@/router'

// create vue application
const app = createApp(App)
app.directive('ripple', Ripple)
app.use(PrimeVue, { ripple: true })

const i18n = setupI18n()
app.use(i18n)

// Homeview and router
const router = setupRouter(i18n)
app.use(router)
app.mount('#app')

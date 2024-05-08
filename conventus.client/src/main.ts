import './assets/main.css'
import 'primevue/resources/themes/lara-light-teal/theme.css'

import { createApp } from 'vue'
import App from './App.vue'

import PrimeVue from 'primevue/config'
import { setupI18n } from './i18n'

// create vue application
const app = createApp(App)

app.use(PrimeVue, { ripple: true })

const i18n = setupI18n()
app.use(i18n)

app.mount('#app')

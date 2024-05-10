import HomeView from '@/components/views/HomeView.vue'
import CategoryView from '@/components/views/CategoryView.vue'
import { createRouter, createMemoryHistory, createWebHistory } from 'vue-router'
import { getLocale, loadLocaleMessages, setI18nLanguage, SUPPORT_LOCALES } from './i18n'
import type { I18n } from 'vue-i18n'
import type { RouteLocationNormalized } from 'vue-router'

export function setupRouter(i18n: I18n) {
    const locale = getLocale(i18n)
    const views = [
        { path: '/:locale/', component: HomeView, name: 'home' },
        { path: '/:locale/:category/', component: CategoryView, name: 'category' },
        { path: '/:pathMatch(.*)*', redirect: () => `/${locale}` }
    ]
    const router = createRouter({
        history: import.meta.env.SSR ? createMemoryHistory() : createWebHistory(),
        routes: views
    })

    // navigation guards
    router.beforeEach(async (to: RouteLocationNormalized) => {
        const paramsLocale = to.params.locale as string

        // use locale if paramsLocale is not in SUPPORT_LOCALES
        if (!SUPPORT_LOCALES.includes(paramsLocale)) {
          return `/${locale}`
        }

        // load locale messages
        if (!i18n.global.availableLocales.includes(paramsLocale)) {
          await loadLocaleMessages(i18n, paramsLocale)
        }

        // set i18n language
        setI18nLanguage(i18n, paramsLocale)
      })

      return router
}


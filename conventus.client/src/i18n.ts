import { nextTick } from 'vue'
import { createI18n, type I18n } from 'vue-i18n'

export const SUPPORT_LOCALES = ['en', 'de']

export function setupI18n(options = {legacy: false, locale: 'en', fallbackLocale: 'en'}): I18n {
    const i18n = createI18n(options)
    setI18nLanguage(i18n, options.locale)
    return i18n
}

export function setI18nLanguage(i18n: I18n, locale: string) {
    if (typeof i18n.global.locale === 'string') {
        i18n.global.locale = locale;
    } else {
        i18n.global.locale.value = locale;
    }
    /**
    * NOTE:
    * If you need to specify the language setting for headers, such as the `fetch` API, set it here.
    * The following is an example for axios.
    *
    * axios.defaults.headers.common['Accept-Language'] = locale
    */
    const htmlElement = document.querySelector('html');
    if (htmlElement) {
        htmlElement.setAttribute('lang', locale);
    }
}

export async function loadLocaleMessages(i18n: I18n, locale: string) {
    // load locale messages with dynamic import
    const messages = await import(
        /* webpackChunkName: "locale-[request]" */ `./locales/${locale}.json`
    )

    // set locale and locale message
    i18n.global.setLocaleMessage(locale, messages.default)

    return nextTick()
}

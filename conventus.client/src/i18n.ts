import { nextTick, isRef } from 'vue'
import { createI18n } from 'vue-i18n'

import type {
  I18n,
  I18nOptions,
  Locale,
  VueI18n,
  Composer,
  I18nMode
} from 'vue-i18n'

export const SUPPORT_LOCALES = ['en', 'de']

function isComposer(
  instance: VueI18n | Composer,
  mode: I18nMode
): instance is Composer {
  return mode === 'composition' && isRef(instance.locale)
}

export function getLocale(i18n: I18n): string {
  if (isComposer(i18n.global, i18n.mode)) {
    return i18n.global.locale.value
  } else {
    return i18n.global.locale
  }
}

export function setLocale(i18n: I18n, locale: Locale): void {
  if (isComposer(i18n.global, i18n.mode)) {
    i18n.global.locale.value = locale
  } else {
    i18n.global.locale = locale
  }
}

export function setupI18n(options: I18nOptions | null = null): I18n {
  if (!options) {
    const browserLanguage = window.navigator.language;
    const userLanguage = browserLanguage.includes('-') ? browserLanguage.split('-')[0] : browserLanguage;
    if (SUPPORT_LOCALES.includes(userLanguage)) {
      options = {
        locale: userLanguage
      }
    } else {
      options = {
        locale: 'en'
      }
    }
  }

  const i18n = createI18n(options)
  setI18nLanguage(i18n, options.locale!)
  loadLocaleMessages(i18n, options.locale!)
  return i18n
}

export function setI18nLanguage(i18n: I18n, locale: Locale): void {
  setLocale(i18n, locale)
  document.querySelector('html')!.setAttribute('lang', locale)
}

// eslint-disable-next-line @typescript-eslint/no-explicit-any
const getResourceMessages = (r: any) => r.default || r

export async function loadLocaleMessages(i18n: I18n, locale: Locale) {
  // load locale messages
  const messages = await import(`./locales/${locale}.json`).then(
    getResourceMessages
  )

  // set locale and locale message
  i18n.global.setLocaleMessage(locale, messages)

  return nextTick()
}

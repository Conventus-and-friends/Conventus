import { useWindowSize } from '@vueuse/core'
import { useRouteParams } from "@vueuse/router";

const { width, height } = useWindowSize()

export function isMobile(): boolean {
    return width.value <= 760
}

export function truncateText(text: string, length: number): string {
    if (text.length <= length) {
        return text
    }
    return text.substring(0, length) + '...'
}

const htmlEntitiesRegex = new RegExp("&(nbsp|amp|quot|lt|gt);", "g")
const htmlEntitiesLookup = {
    "&nbsp;": ' ',
    "&amp;": '&',
    "&quot;": '"',
    "&lt;": '<',
    "&gt;": '>'
}

export function removeHtmlEntities(text: string): string {
    return text.replace(htmlEntitiesRegex, (match) => htmlEntitiesLookup[match as keyof typeof htmlEntitiesLookup])
}

export function dateAsUtcDate(date: Date): Date {
    return new Date(Date.UTC(date.getFullYear(), 
                                date.getMonth(), 
                                date.getDate(), 
                                date.getHours(), 
                                date.getMinutes(), 
                                date.getSeconds()));
}
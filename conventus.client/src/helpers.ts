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
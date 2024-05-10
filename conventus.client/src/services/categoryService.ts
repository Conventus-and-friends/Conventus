import type { Category } from "@/models/category";

export function getCategories(): Promise<Category[]> {
    return fetch("/api/categories")
        .then(response => response.json())
        .then(json => json as Category[])
}

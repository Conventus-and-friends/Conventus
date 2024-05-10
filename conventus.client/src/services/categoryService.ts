import type { Category } from "@/models/category";

export function getCategories(): Promise<Category[]> {
    return fetch("/api/categories")
        .then(response => response.json())
        .then(json => json as Category[])
}

export function getCategory(id: number): Promise<Category> {
    return fetch(`/api/categories/${id}`)
        .then(response => response.json())
        .then(json => json as Category)
}

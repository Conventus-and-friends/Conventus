import type { Category } from "@/models/category";

/**
 * Retrieves all categories from the API.
 *
 * @return {Promise<Category[]>} A promise that resolves to an array of Category objects.
 */
export function getCategories(): Promise<Category[]> {
    return fetch("/api/categories")
        .then(response => response.json())
        .then(json => json as Category[])
}

/**
 * Retrieves a category by its ID from the API.
 *
 * @param {number} id - The ID of the category to retrieve.
 * @return {Promise<Category | null>} A promise that resolves to the category object if found, or null if not found.
 */
export function getCategory(id: number): Promise<Category | null> {
    return fetch(`/api/categories/by-id/${id}`)
        .then(response => response.ok ? response.json() : null)
        .then(data =>  data as Category ?? null)
}

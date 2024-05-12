import type { Post } from "@/models/post";

/**
 * Retrieves a list of posts based on the specified category, page, and length.
 *
 * @param {number} category - The category ID to filter posts.
 * @param {number} page - The page number to retrieve.
 * @param {number} length - The number of posts per page.
 * @return {Promise<Post[]>} A promise that resolves to an array of Post objects.
 */
export function getPosts(category: number, page: number, length: number): Promise<Post[]> {
    return fetch(`/api/posts/by-category/${category}?` + new URLSearchParams({
        page: page.toString(),
        length: length.toString()
    }))
        .then(response => response.json())
        .then(json => json as Post[])
}

/**
 * Retrieves the total number of pages for posts based on the specified category and length.
 *
 * @param {number} category - The category ID to filter posts.
 * @param {number} length - The length of posts per page.
 * @return {Promise<number>} A promise that resolves to the total number of pages.
 */
export function getPostsPageCount(category: number, length: number): Promise<number> {
    return fetch(`/api/posts/by-category/${category}?` + new URLSearchParams({
        length: length.toString()
    }))
        .then(response => response.json())
        .then(json => json as number)
}

/**
 * Retrieves a post by its ID from the API.
 *
 * @param {string} id - The ID of the post to retrieve.
 * @return {Promise<Post | null>} A promise that resolves to the post object if found, or null if not found.
 */
export function getPost(id: string): Promise<Post | null> {
    return fetch(`/api/posts/by-id/${id}`)
        .then(response => response.ok ? response.json() : null)
        .then(data =>  data as Post ?? null)
}
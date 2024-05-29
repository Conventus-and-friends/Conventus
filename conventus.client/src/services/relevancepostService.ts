import type { Relevance } from "@/models/relevancepost";

/**
 * Retrieves all relevance posts from the API.
 *
 * @param {number} limit - The maximum number of posts to retrieve.
 * @return {Promise<Relevance[]>} A promise that resolves to an array of relevance post objects.
 */
export function getRelevancePost(limit: number): Promise<Relevance[]> {
    return fetch(`/api/posts/by-relevance?limit=${limit}`)
        .then(response => response.json())
        .then(json => json as Relevance[])
}

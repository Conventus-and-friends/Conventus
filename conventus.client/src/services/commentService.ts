import { recreateDate } from "@/helpers";
import type { Comment } from "@/models/comment";

export function getComments(post: string, page: number, length: number): Promise<Comment[]> {
    return fetch(`/api/comments/by-post/${post}?` + new URLSearchParams({
        page: page.toString(),
        length: length.toString()
    }))
        .then(response => response.json())
        .then(json => json as Comment[])
}

export function getCommentsCount(post: string): Promise<number> {
    return fetch(`/api/comments/by-post/${post}/count`)
        .then(response => response.json())
        .then(json => json as number)
}

export function getPost(id: string): Promise<Comment | null> {
    return fetch(`/api/comments/by-id/${id}`)
        .then(response => response.ok ? response.json() : null)
        .then(data =>  data as Comment ?? null)
        .then((comment: Comment | null) => {
            if (comment?.created) {
                comment.created = recreateDate(comment.created);
            }
            return comment;
        })
}

export function newComment(comment: Comment): Promise<Comment | null> {
    return fetch("/api/comments", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(comment)
    })
        .then(response => response.ok ? response.json() : null)
        .then(json => json as Comment ?? null)
}

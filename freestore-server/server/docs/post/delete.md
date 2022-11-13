# Delete a post

Allow user to delete a comment under certain post.

**URL** : `/api/v1/categories/{id}/item/{id}`

**Method** : `DELETE`

**Auth required** : YES

**Permissions required** : Admin, Resource is owned by user

## Success Responses

**Code** : `204 No Content`

If deletion is successful no content will be returned

## Error responses

**Code** : `404 Not Found`

If item within existing category was not found, following message will be returned

```json
{
    "message": "Item was not found"
}
```
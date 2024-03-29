# Unlist a category

Allow user to unlist and prevent creating posts under the category

**URL** : `/api/v1/categories/{id}`

**Method** : `DELETE`

**Auth required** : YES

**Permissions required** : Admin

## Success Responses

**Code** : `204 No Content`

If category is successfully unlisted no content will be returned

## Error responses

**Code** : `404 Not Found`

If requested with identifier with which no category is associated 404 response will be returned.

```json
{
    "message": "Category is not found"
}
```

**Code** : `403 Forbidden`

If tried to access as a regular user and not as a system administrator
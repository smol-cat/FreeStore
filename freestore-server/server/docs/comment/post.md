# Create a new comment

Allow user to create a new comment

**URL** : `/api/v1/categories/{id}/items/{id}/comment`

**Method** : `POST`

**Auth required** : YES

**Permissions required** : None

**Data constraints**

- Text field is provided;

```json
{
    "text": "Kiek metu saldytuvui?"
}
```

## Success Responses

**Code** : `201 Created`

**Content example** : 

If all validation requirements are met - success response.

```json
{
    "message": "Comment has been posted"
}
```
## Error responses

**Code** : `404 Not Found`

If requested with category or item id which does not exist in the system following message will be returned:

```json
{
    "message": "Item or category was not found"
}
```

**Code** : `400 Bad Request`

If text is empty:

```json
{
    "message": "Title should be included"
}
```


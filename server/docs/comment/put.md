# Edit a comment

Allow user to edit a comment

**URL** : `/api/v1/categories/{id}/items/{id}/comment/{id}`

**Method** : `PUT`

**Data constraints**

- Text field is provided;

```json
{
    "text": "Kiek metu saldytuvui?"
}
```

## Success Responses

**Code** : `200 OK`

**Content example** : 

If all validation requirements are met - success response.

```json
{
    "message": "Comment has been updated"
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


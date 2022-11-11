# Get comment information

Allow user to get information of a specific comment.

**URL** : `/api/v1/categories/{id}/items/{id}/comments/{id}`

**Method** : `GET`

**Auth required** : YES

**Permissions required** : None

## Success Responses

**Code** : `200 OK`

**Content example** : 

Response will return information of comment.

```json
{
    "id": 3,
    "text": "Kiek metu saldytuvui?",
    "date_created": "2022-10-08T08:15:00",
    "account": {
        "id": 1,
        "name": "Ernestas",
        "last_name": "Untulis"
    }
}
```
## Error responses

**Code** : `404 Not Found`

If comment with provided id hierarchy was not found, following message will be returned

```json
{
    "message": "Comment was not found"
}
```
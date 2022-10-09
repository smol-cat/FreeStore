# Get list of comments

Allow user to get a list of comments under a post.

**URL** : `/api/v1/categories/{id}/items/{id}/comments`

**Method** : `GET`

## Success Responses

**Code** : `200 OK`

**Content example** : 

Response will provide a list of comments

```json
[
    {
        "id": 1,
        "text": "Kietai",
        "date_created": "2022-10-08T07:38:00",
        "account": {
            "id": 1,
            "name": "Ernestas",
            "last_name": "Untulis"
        }
    },
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
]
```
## Error responses

**Code** : `404 Not Found`

If requested with category or item id which does not exist in the system following message will be returned:

```json
{
    "message": "Item or category was not found"
}
```
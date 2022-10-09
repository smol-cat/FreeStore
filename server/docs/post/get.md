# Get post information

Allow user to get information of a specific post.

**URL** : `/api/v1/categories/{id}/items/{id}`

**Method** : `GET`

## Success Responses

**Code** : `200 OK`

**Content example** : 

Response will return information of post.

```json
{
    "id": 2,
    "title": "Telefonas",
    "description": "Samsung Galaxy",
    "price": 251.00,
    "image_ids": [],
    "category": {
        "id": 1,
        "name": "Maistas"
    },
    "account": {
        "id": 1,
        "name": "Ernestas",
        "last_name": "Untulis"
    },
    "state": {
        "id": 1,
        "name": "Parduodama"
    }
}
```
## Error responses

**Code** : `404 Not Found`

If requested with category or item id which does not exist in the system.

```json
{
    "message": "Item or category was not found"
}
```
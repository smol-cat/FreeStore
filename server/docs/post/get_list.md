# Get list of posts

Allow user to get a list of posts of specific category.

**URL** : `/api/v1/categories/{id}/item`

**Method** : `GET`

## Success Responses

**Code** : `200 OK`

**Content example** : 

Response will provide a list of items of specific category

```json
[
    {
        "id": 1,
        "title": "Šaldytuvas",
        "description": "Didelis šaldytuvas",
        "price": 200.00,
        "image_ids": [
            2,
            3
        ],
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
    },
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
]
```

## Error Responses


**Code** : `404 Not Found`

If requested with category which does not exist in the system following message will be returned:

```json
{
    "message": "Category was not found"
}
```
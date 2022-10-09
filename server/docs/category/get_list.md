# Get list of categories

Allow user to get a list of all categories.

**URL** : `/api/v1/categories`

**Method** : `GET`

## Success Responses

**Code** : `200 OK`

**Content example** : 

Response will provide a list of all categories

```json
[
    {
        "id": 1,
        "name": "Maistas",
        "description": "Įvairūs maisto produktai"
    },
    {
        "id": 2,
        "name": "Kompiuterinė įranga",
        "description": "Kompiuteriai, telefonai, bei jų komponentai"
    }
]
```
# Edit a post post

Allow user to edit a post

**URL** : `/api/v1/categories/{id}/items/{id}`

**Method** : `PUT`

**Auth required** : YES

**Permissions required** : Resource is owned by user

**Data constraints**

- All fields are provided;
- Fields are not empty;
- Provided price is not negative.

```json
{
    "title": "Naujas kompiuteris",
    "description": "Lenovo Legion Y580 GTX RTX...",
    "price": 2000,
    "status_id": 1,
    "category_id": 2
}
```

## Success Responses

**Code** : `200 OK`

**Content example** : 

If all validation requirements are met - success response.

```json
{
    "message": "Item information has been updated"
}
```
## Error responses

**Code** : `404 Not Found`

If provided category or item id does not exist following message will be returned:

```json
{
    "message": "Category or item was not found"
}
```

**Code** : `400 Bad Request`

If one of the fields are missing or empty.

If title is empty

```json
{
    "message": "Title should be included"
}
```

If price is not provided, null or negative:

```json
{
    "message": "Price is invalid or not provided"
}
```

If status information is not provided, null or negative:

```json
{
    "message": "Status id is empty or not provided"
}
```


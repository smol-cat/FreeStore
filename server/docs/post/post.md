# Create a new post

Allow user to create a new post

**URL** : `/api/v1/categories/{id}/items`

**Method** : `POST`

**Auth required** : YES

**Permissions required** : None

**Data constraints**

- All fields are provided;
- Fields are not empty;
- Provided price is not negative.

```json
{
    "title": "Naujas kompiuteris",
    "description": "Lenovo Legion Y580 GTX RTX...",
    "price": 2000
}
```

## Success Responses

**Code** : `201 Created`

**Content example** : 

If all validation requirements are met - success response.

```json
{
    "message": "Item has been posted"
}
```
## Error responses

**Code** : `404 Not Found`

If category under which item is wanted to post does not exist or it is unlisted it will a following message:

```json
{
    "message": "Category was not found"
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


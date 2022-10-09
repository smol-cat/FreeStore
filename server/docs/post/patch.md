# Unlist or update post status

Allow user to unlist or change status of a post

**URL** : `/api/v1/categories/{id}/item/{id}`

**Method** : `PATCH`

**Data constraints**

- At least one of these fields are required;
- If `unlist` field has true value, item will be unlisted without changing its state;
- If `unlist` is false or it is not provided, item state will be changed with provided `status_id`.

```json
{
    "status_id": 2,
    "unlist": true
}
```

## Success Responses

**Code** : `200 OK`

If item is unlisted following message will be returned:

```json
{
    "message": "Item has been unlisted"
}
```

If item status was updated following message will be returned:

```json
{
    "message": "Item status was updated"
}
```

## Error responses

**Code** : `404 Not Found`

If requested with identifiers with which no category or item is associated or one of these resources are unlisted, 404 response will be returned.

```json
{
    "message": "Item or category was not found"
}
```
**Code** : `400 Bad Request`

If no fields were provided or there is only `unlist` token with value `false` following message will be returned:

```json
{
    "message": "Patch information was not provided"
}
```
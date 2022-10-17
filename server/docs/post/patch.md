# Update post status

Allow user to unlist or change status of a post

**URL** : `/api/v1/categories/{id}/item/{id}`

**Method** : `PATCH`

**Data constraints**

- Status id field is required;

```json
{
    "status_id": 2,
}
```

## Success Responses

**Code** : `200 OK`

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

If no fields were provided following message will be returned:

```json
{
    "message": "Status information was not provided"
}
```
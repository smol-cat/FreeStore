# Block an account

Allow user to block an account

**URL** : `/api/v1/accounts/{id}`

**Method** : `PATCH`

## Success Responses

**Code** : `204 No Content`

If block is successful no content will be returned

## Error responses

**Code** : `404 Not Found`

If requested with identifier with which no account is associated 404 response will be returned.

```json
{
    "message": "Account is not found"
}
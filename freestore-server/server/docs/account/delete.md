# Block an account

Allow user to block an account

**URL** : `/api/v1/accounts/{id}`

**Method** : `DELETE`

**Auth required** : YES

**Permissions required** : Admin, User is account owner

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
```

**Code**: `403 Forbidden`

If you are not an admin and try to delete an account which you do not own.
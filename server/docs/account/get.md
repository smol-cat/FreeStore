# Get specific account

Allow user to get information of a specific user.

**URL** : `/api/v1/accounts/{id}`

**Method** : `GET`

## Success Responses

**Code** : `200 OK`

**Content example** : 

If requested with `{id} = 1` response will return information of user with identifier 1. 

```json
{
    "id": 1,
    "name": "Ernestas",
    "last_name": "Untulis",
    "date_created": "2022-10-05T04:32:00"
}
```
## Error responses

**Code** : `404 Not Found`

If requested with identifier with which no account is associated 404 response will be returned.

```json
{
    "message": "Account is not found"
}
```
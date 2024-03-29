# Edit account details

Allow user to edit their account details

**URL** : `/api/v1/accounts`

**Method** : `PUT`

**Auth required** : YES

**Data constraints**

Provide all required fields.

```json
{
    "name": "Ernestas",
    "lastName": "Untulis",
    "phoneNumber": "+861234568"
}
```

## Success Responses

**Code** : `200 OK`

**Content example** : 

If name and last name contain proper characters - success response.

```json
{
    "message": "Account has been updated"
}
```
## Error responses

**Code** : `400 Bad Request`

If one of the fields are missing (except for phone number) 400 response will be returned.

If email is incorrect

```json
{
    "message": "Account associated with this email is not found"
}
```

If password is incorrect

```json
{
    "message": "Name or last name is empty or contain invalid characters"
}
```


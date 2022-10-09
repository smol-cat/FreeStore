# Login to account

Allow user to login to their account

**URL** : `/api/v1/accounts/login`

**Method** : `POST`

**Data constraints**

Provide correct email and password.

```json
{
    "email": "ernestasunt@gmail.com",
    "password": "hesdvsvfghfdy"
}
```

## Success Responses

**Code** : `200 OK`

**Content example** : 

If email and password are correct you will be logged in to your account

```json
{
    "message": "You've logged in"
}
```
## Error responses

**Code** : `400 Bad Request`

If one of the fields are missing 400 response will be returned.

If all required fields are filled but email or password are incorrect 406 response will be returned.

If email is incorrect

```json
{
    "message": "Account associated with this email is not found"
}
```

If password is incorrect

```json
{
    "message": "Password is incorrect"
}
```


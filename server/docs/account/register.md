# Register a new account

Allow user to register to the system

**URL** : `/api/v1/accounts/register`

**Method** : `POST`

**Data constraints**

- All fields are provided;
- Name and last name should not contain any special characters;
- Email follow email format;
- Password is not shorter than 8 characters;
- There is no existing account with provided email address.

```json
{
    "name": "Ernestas",
    "lastName": "Untulis",
    "email": "ernestasunt@gmail.com",
    "password": "hesdvsvfghfdy"
}
```

## Success Responses

**Code** : `201 Created`

**Content example** : 

If all validation requirements are met - success response.

```json
{
    "message": "Account has been created"
}
```
## Error responses

**Code** : `400 Bad Request`

If one of the fields are missing 400 response will be returned.

If name or last name are empty or contain invalid characters.

```json
{
    "message": "Name or last name is empty or contain invalid characters"
}
```

If password is too short

```json
{
    "message": "Password should contain at least 8 characters"
}
```

If account with provided email address exists

```json
{
    "message": "Account with this email already exists"
}
```


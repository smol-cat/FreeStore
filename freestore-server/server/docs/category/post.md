# Create a new category

Allow user to create a new category

**URL** : `/api/v1/categories`

**Method** : `POST`

**Auth required** : YES

**Permissions required** : Admin

**Data constraints**

- All fields are provided;
- Fields are not empty.

```json
{
    "name": "Namų technika",
    "description": "Elektrinė namų, virtuvės ir t.t. technika"
}
```

## Success Responses

**Code** : `201 Created`

**Content example** : 

If all validation requirements are met - success response.

```json
{
    "message": "Category is created"
}
```
## Error responses

**Code** : `400 Bad Request`

If one of the fields are missing or empty.

```json
{
    "message": "Fields: description and name are required"
}
```

**Code** : `403 Forbidden`

If tried to access as a regular user and not as a system administrator

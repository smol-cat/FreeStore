# Edit a category

Allow user to edit category information

**URL** : `/api/v1/categories/{id}`

**Method** : `PUT`

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

**Code** : `20O OK`

**Content example** : 

If all validation requirements are met - success response.

```json
{
    "message": "Category information has been updated"
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


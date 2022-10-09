# Get list of accounts

Allow user to get a list of all registered accounts.

**URL** : `/api/v1/accounts`

**Method** : `GET`

## Success Responses

**Code** : `200 OK`

**Content example** : 

Response will provide a list of all registered users

```json
[
    {
        "id": 1,
        "name": "Ernestas",
        "last_name": "Untulis",
        "date_created": "2022-10-05T04:32:00"
    },
    {
        "id": 4,
        "name": "Juozas",
        "last_name": "Petrauskas",
        "date_created": "2022-10-08T02:52:00"
    }
]
```

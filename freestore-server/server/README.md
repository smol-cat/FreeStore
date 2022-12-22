# FreeStore API documentation

This is the documentation of the FreeStore system. Below is the list of all endpoints with links which lead to specifics and requirements for each endpoint. 

## Open Endpoints

Open endpoints which require no authentication to be accessable.

### Accounts related

* [Register](docs/account/register.md) : `POST /api/v1/accounts/register`

### Authentication tokens related

* [Login](docs/token/post.md) : `POST /api/v1/tokens`

### Posts category related

* [List of categories](docs/category/get_list.md) : `GET /api/v1/categories`

## Endpoints that require authentication

These are the endpoints which can be accessed by any authenticated users.

### Accounts related

* [Get account by id](docs/account/get.md) : `GET /api/v1/accounts/{id}`
* [Block account](docs/account/delete.md) : `PATCH /api/v1/accounts/{id}`
* [Edit account](docs/account/put.md) : `PUT /api/v1/accounts`

### Authentication tokens related

* [Logout](docs/token/delete.md) : `DELETE /api/v1/tokens`

### Posts related

* [List of posts](docs/post/get_list.md) : `GET /api/v1/categories/{id}/item`
* [Get post by id](docs/post/get.md) : `GET /api/v1/categories/{id}/item/{id}`
* [Create new post](docs/post/post.md) : `POST /api/v1/categories/{id}/item/`
* [Edit post](docs/post/put.md) : `PUT /api/v1/categories/{id}/item/`
* [Delete post](docs/post/delete.md) : `DELETE /api/v1/categories/{id}/item/{id}`

### Comments related

* [List of comments](docs/comment/get_list.md) : `GET /api/v1/categories/{id}/item/{id}/comments`
* [Get comment by id](docs/comment/get.md) : `GET /api/v1/categories/{id}/item/{id}/comments/{id}`
* [Post new comment](docs/comment/post.md) : `POST /api/v1/categories/{id}/item/{id}/comments`
* [Edit comment](docs/comment/put.md) : `PUT /api/v1/categories/{id}/item/{id}/comments/{id}`
* [Delete comment](docs/comment/delete.md) : `DELETE /api/v1/categories/{id}/item/{id}/comments/{id}`

## Endpoints that require admin authorization

These are the endpoints which can be accessed only by a system administrator.

### Accounts related

* [List of accounts](docs/account/get_list.md) : `GET /api/v1/accounts`

### Posts category related

* [Create new category](docs/category/post.md) : `POST /api/v1/categories`
* [Edit category](docs/category/put.md) : `PUT /api/v1/categories/{id}`
* [Delete category](docs/category/delete.md) : `DELETE /api/v1/categories/{id}`

## Notes

If tried to access any of the endpoints which require authentication without being authenticated you will receive a response with a status code of `401 Unauthorized`.
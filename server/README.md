# ASP.NET API documentation

This is the documentation of the FreeStore system. Below is the list of all endpoints with links which lead to specifics and requirements for each endpoint. 

## Open Endpoints

Currently all endpoints are open since there is no authentication/authorization implemented.

### Account related

* [List of accounts](docs/account/get_list.md) : `GET /api/v1/accounts`
* [Get account by id](docs/account/get.md) : `GET /api/v1/accounts/{id}`
* [Block account](docs/account/patch.md) : `PATCH /api/v1/accounts/{id}`
* [Edit account](docs/account/put.md) : `PUT /api/v1/accounts`
* [Login](docs/account/login.md) : `POST /api/v1/accounts/login`
* [Register](docs/account/register.md) : `POST /api/v1/accounts/register`

### Posts category related

* [List of categories](docs/category/get_list.md) : `GET /api/v1/categories`
* [Create new category](docs/category/post.md) : `POST /api/v1/categories`
* [Edit category](docs/category/put.md) : `PUT /api/v1/categories/{id}`
* [Unlist category](docs/category/patch.md) : `PATCH /api/v1/categories/{id}`

### Posts related

* [List of posts](docs/post/get_list.md) : `GET /api/v1/categories/{id}/item`
* [Get post by id](docs/post/get.md) : `GET /api/v1/categories/{id}/item/{id}`
* [Create new post](docs/post/post.md) : `POST /api/v1/categories/{id}/item/`
* [Edit post](docs/post/put.md) : `PUT /api/v1/categories/{id}/item/`
* [Unlist or change status](docs/post/patch.md) : `PATCH /api/v1/categories/{id}/item/{id}`

### Comments related

* [List of comments](docs/comment/get_list.md) : `GET /api/v1/categories/{id}/item/{id}/comments`
* [Get comment by id](docs/comment/get.md) : `GET /api/v1/categories/{id}/item/{id}/comments/{id}`
* [Post new comment](docs/comment/post.md) : `POST /api/v1/categories/{id}/item/{id}/comments`
* [Edit comment](docs/comment/put.md) : `PUT /api/v1/categories/{id}/item/{id}/comments/{id}`
* [Delete comment](docs/comment/delete.md) : `DELETE /api/v1/categories/{id}/item/{id}/comments/{id}`

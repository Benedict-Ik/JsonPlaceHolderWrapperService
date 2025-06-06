# Json Placeholder Wrapper Service 

## Project Goal
The goal of this project was to build a wrapper service that interacted with a public API, exposing simplified, secure endpoints to downstream consumers.  

A **wrapper service** is an API that acts as an intermediary, consuming and abstracting another API to provide a simplified, standardized, or modified interface for clients.

## API Used
For this project, we made use of Json Placeholder which allows developers to simulate API responses and test their applications without having to create a real backend or worry about data storage. 

## Key Endpoints for Our Wrapper API
|    Endpoint         |    Action    |
|---------------------|--------------|
| GET/api/posts  | Retrieve all posts|
| GET/api/posts/\{id}| Fetches a single post by ID
| GET/api/posts/\{id}/comments| Get comments for a specific post
| GET/api/users/\{id} | Retrieves details for a specific user

## Example API Documentation
**Endpoint: Get All Posts**
- URL: /api/posts
- Method: GET
- Description: Returned a list of all posts.

**Response:**
```JSON
[
  {
    "userId": 1,
    "id": 1,
    "title": "Sample title",
    "body": "Sample body"
  },
  ...
]
```  

**Endpoint: Get a Single Post**
- URL: /api/posts/{id}
- Method: GET
- URL Parameters:
    - id (integer) – ID of the post

**Response:**
```JSON
{
  "userId": 1,
  "id": 1,
  "title": "Sample title",
  "body": "Sample body"
}
``` 

**Endpoint: Get Comments for a Post**
- URL: /api/posts/{id}/comments
- Method: GET
- URL Parameters:
    - id (integer) – ID of the post

**Response**
```JSON
[
  {
    "postId": 1,
    "id": 1,
    "name": "Commenter Name",
    "email": "email@example.com",
    "body": "Comment body"
  },
  ...
]
```

**Endpoint: Get User Details**
- URL: /api/users/{id}
- Method: GET
- URL Parameters:
    - id (integer) – ID of the user

**Response**
```JSON
{
  "id": 1,
  "name": "User Name",
  "username": "username",
  "email": "email@example.com",
  "address": {
    "street": "Kulas Light",
    "city": "Gwenborough",
    "zipcode": "92998-3874"
  },
  "phone": "1-770-736-8031",
  "website": "hildegard.org",
  "company": {
    "name": "Romaguera-Crona",
    "catchPhrase": "Multi-layered client-server neural-net",
    "bs": "harness real-time e-markets"
  }
}
```

## Authentication/Authorization
- Since the `JSONPlaceholder API` is a public API used for testing purposes, it doesn't require authentication to use its endpoints.  
- We secured the wrapper API with basic authentication (username/password).
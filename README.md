# Json Placeholder Wrapper Service 
An irregularity was observed upon calling various endpoints. They returned empty data, as seen below.
```JSON
[
  {
    "userId": 0,
    "id": 0,
    "title": "",
    "body": ""
  },
  {
    "userId": 0,
    "id": 0,
    "title": "",
    "body": ""
  },
  {
    "userId": 0,
    "id": 0,
    "title": "",
    "body": ""
  },
  ...
]
```

Upon troubleshooting, it was discovered that my Models and the API's object names did not match when deserializing. The specific error was termed **Case-Sensitive** Deserialization.  
To resplve the issue, I modified the service class to ensure `PropertyNameCaseInsensitive` is set to `true` when deserializing.  
Case sample:
```C#
var response = await httpClient.GetAsync("https://jsonplaceholder.typicode.com/posts");
response.EnsureSuccessStatusCode();

var json = await response.Content.ReadAsStringAsync();
var posts = JsonSerializer.Deserialize<List<Post>>(json, new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
});
```
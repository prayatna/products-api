# ProductAPI

Public API to perform basic CRUD operations for Products and its options.

ProductApi target framework has been upgraded to .NET 5. Open solution and build through Visual Studio, the launch url points to Swagger UI which lists all the endpoints in more detail.

## Folder Structure

```
.
├── Data
│   ├── Migrations  # auto generated migrations
│   ├── Repositories
├── Domain
│   ├── Entity # model that matches the database table
│   ├── Interfaces # interfaces to repositories
├── WebApi
│   ├── App_Data
│   ├── Controllers
│   ├── Dto # data transfer objects to API request/response
│   ├── Helpers
│   ├── Services # service that connects Data layer to API
├── WebApi.Tests # unit tests

```

## Current Endpoints

There are following endpoints in use:

1. `GET /products` - gets all products.
2. `GET /products?name={name}` - finds all products matching the specified name.
3. `GET /products/{id}` - gets the project that matches the specified ID - ID is a GUID.
4. `POST /products` - creates a new product.
5. `PUT /products/{id}` - updates a product.
6. `DELETE /products/{id}` - deletes a product and its options.
7. `GET /products/{id}/options` - finds all options for a specified product.
8. `GET /products/{id}/options/{optionId}` - finds the specified product option for the specified product.
9. `POST /products/{id}/options` - adds a new product option to the specified product.
10. `PUT /products/{id}/options/{optionId}` - updates the specified product option.
11. `DELETE /products/{id}/options/{optionId}` - deletes the specified product option.

All data transfer objects are specified in the `WebApi/Dto` folder, and conform to:

**Product:**

```
{
  "Id": "01234567-89ab-cdef-0123-456789abcdef",
  "Name": "Product name",
  "Description": "Product description",
  "Price": 123.45,
  "DeliveryPrice": 12.34
}
```

**List of Products:**

```
{
  "Items": [
    {
      // product
    },
    {
      // product
    }
  ]
}
```

**Product Option:**

```
{
  "Id": "01234567-89ab-cdef-0123-456789abcdef",
  "Name": "Product name",
  "Description": "Product description"
}
```

**List of Product Options:**

```
{
  "Items": [
    {
      // product option
    },
    {
      // product option
    }
  ]
}
```

More detailed data structure can be viewed from the Swagger UI.

The project has been divided into Api - Domain - Data Layer. Data layer consists of repository that can access the database, and the migrations used to create tables. Domain layer is not dependent to any other layer. This layer holds the key entity models, and the interface (action required to perform business logic) that is required for the application. Api Layer consists of all the endpoints, and services that connects the Data layer to the application.

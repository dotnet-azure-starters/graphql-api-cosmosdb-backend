# GraphQL API With Cosmos DB Backend 
## What is the purpose of this template?
One stop template for anyone wanting to create a GraphQL API in .NET that connects to multiple CosmosDb instances to fetch data.

### Prerequisites
- [What is GraphQL]
- [What is Hot Chocolate GraphQL Server For .NET]
- [What is Cosmos DB]
- [What is schema stitching] (when merging data from multiple Cosmos DB instances)
- [GraphQL schema first design] (design your schema first and then develop the code to fulfil the schema)

### Features
- Schema first GraphQL .NET API
- Connects to multiple CosmosDb instances to get data
- Integrates to Azure App Insights for all logging and telemetry
- Full scaffolding setup with resolvers for schema first GraphQL queries
- Scaffolding for data schema and Cosmos DB repositories
- Scaffolding for domain model
- Domain model to data model mapping
- Paging support when returning a large data set

### TODO
- Add support for OAuth
- Implement the repository pattern when connecting to multiple Cosmos DB's
- Add support to connect to other data sources such as SQL Server, MySQL etc


> Please be aware, this is still work in progress and not ready for a production scenario

> Any contributions or input will be much appreciated

[//]: # (These are reference links used in the body of this note and get stripped out when the markdown processor does its job. There is no need to format nicely because it shouldn't be seen. Thanks SO - http://stackoverflow.com/questions/4823468/store-comments-in-markdown-syntax)

   [What is GraphQL]: <https://graphql.org/>
   [What is Hot Chocolate GraphQL Server For .NET]: <https://chillicream.com/docs/hotchocolate>
   [What is Cosmos DB]: <https://docs.microsoft.com/en-us/azure/cosmos-db/introduction>
   [What is schema stitching]: <https://chillicream.com/docs/hotchocolate/v10/stitching>
   [GraphQL schema first design]: <https://www.apollographql.com/blog/graphql/basics/designing-your-first-graphql-schema/>

## How to run project locally:

Prerequisites:
- Docker desktop installed and running
- .NET 6 SDK installed
- dotnet ef tools. Command: `dotnet tool install --global dotnet-ef`
- IDE needed (Visual Studio, Rider, VSCode)

1. Navigate to main project's folder in your terminal
2. Run `docker-compose up` command to run SQL Server DB in a container
3. Apply migrations to DB using this command: `dotnet ef database update -p ./InsurancePolicyService.Infrastructure -s ./InsurancePolicyService.API -- --environment Development`. Note: relative paths might need to be changed if you are on Windows
4. Run InsurancePolicyService.API project (web API). You can use either IIS Express or Kernel

## How to consume our API:
- Use our Swagger page to find all our available endpoints and their required data
- If there's an error, the API will return this object:
```json
{
  "errorMessage": string
}
```

## If the State Regulations method is getting very complicated and a separate team is being started to own its requirements and code, what would you recommend to make that transition smoother?

- Define how this part will be extracted (nuget package, new API, etc)
- Define what information should be sent to the new service and returned from it.
- All the service does in this API is abstracted using an interface. Therefore, only a new concrete class would need to be created. 

## If you were to hand the code off to a new Engineering team for ownership. How should they think about productionizing it? (Monitoring, Logging, safe handling of PII, extending methods)

- Change logging provider since it is using the console. They could use Elasticsearch, Azure App Insights or any other. All this can be achieved in Program.cs file. Use structured logging.
- Add middleware for logging requests and responses
- Mask sensitive information when logging requests and responses
- Add authentication using either JWTs or subscriptions keys depending who is consuming the API. 
- If service is only consumed by other internal services and not by any external client or if there's a API gateaway:
  - Whitelist IP addresses that will access to the API
  - Do not use HTTPS protocol
- If queries get more and more complex, use Dapper for all DBs operations. Only changes in Infrastructure project will be needed (new repository concrete classes and register them in DI)
- Monitoring can be achieved with the cloud provider used for deploying API


## Considerations when deploying to a cloud provider
- Create one `appsettings.json` file for each environment. IMPORTANT: Do not include secrets.
- Use cloud provider for storing secrets (Azure: Key Vault, AWS: Key Management Service). Secrets should be loaded in `Program.cs` class and injected into `IConfiguration`. Credentials for connecting to this secrets store can be provided in the Release pipeline.
- Create two branches, `development` and `release`. All PRs will go first to `development`and then, once we need to push to higher environments (QA/Stage/Prod) will make another PR from `development` to `release`. This way we can control what will be released at the end of each sprint.
- Create CI/CD pipelines. You can do this in Azure DevOps pretty easily. One pipeline will be for development (DEV env) and another one for all the rest of the environments
- API can be deployed in Azure App Service or in AWS Elastic Beanstalk. 
- We could also use Kubernetes and containerized our API. If this is the case, a Dockerfile should be created along with all the configuration files Kubernetes needs. CI/CD pipelines will need some modifications
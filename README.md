# Best Hacker News Api

## Notes on design & implementation decisions

- This app uses Polly's bulkhead policy to limit concurrent requests to third party (Hacker News) API
- Polly is also used as a resilence layer for third party API calls (retry policy)
- The responses from Hacker News API are cached with help of custom caching component which uses IMemoryCache (local cache). As a result first call is slow, then subsequent calls to the API are fast
- See configuration param in appsettings for controlling cache timeout: CacheAbsoluteExpiration, default value 60 seconds
- Polly's caching feature is not used in this implementation as custom cache implementation can cache smaller objects (only used fields are deserialized and cached in this case)

## Potential future enhancements

- Enhance e2e/integration tests and introduce e.g. WireMock in order to mock third party API calls
- Depending on desired deployment scenario, in order to maximize scalability, consider using IDistributedCache together with a distributed cache instance of your choice, instead of local IMemoryCache

## Execution
- One can start the app from Visual Studio - e.g. open solution BestHNewsApi.sln and start BestHNewsApi project with 'http' profile, this should open a Swagger page at http://localhost:5120/swagger/index.html
- Docker image build is present and can be used as well
- BestHNewsApi.IntegrationTests require an app running locally

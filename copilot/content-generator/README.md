# Dependencies

```sh
docker run --name redis -e ALLOW_EMPTY_PASSWORD=yes -p 6379:6379 bitnami/redis:6.2
```

For local development, this project requires a Redis server running on port 6379. You can start a Redis instance using the provided Docker command. The application will connect to this Redis server using the default endpoint and port specified in the `.env` file.

```sh
pip install -r requirements.txt
```

# Tests

To run the unit tests and generate a coverage report, use the following commands:

```sh
# Install test requirements
pip install -r test_requirements.txt

# Run tests with coverage
pytest --cov=.
```

# Environment Variables

The following environment variables are used in this project:

- `APPLICATIONINSIGHTS_CONNECTION_STRING`: Connection string for Azure Application Insights.
- `ENABLE_APP_INSIGHTS`: Flag to enable or disable Azure Application Insights (default: `false`).
- `LOG_LEVEL`: Logging level (default: `DEBUG`).
- `REDIS_PORT`: Port for the Redis server (default: `6379`).
- `REDIS_URL`: URL for the Redis server (default: `localhost`).

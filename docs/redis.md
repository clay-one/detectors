[Table of Contents](toc.md)

# Detectors - Redis module

The Redis module allows you to connect to one (or more) Redis servers and query its status and content.

## Configuration

This is the Redis part from `connections.json` file:

    "redis": {
      "connections": [
        {
          "id": "connection-one",
          "endPoints": ["server-one:1234", "server-two:5678"]
        },
        {
          "id": "connection-two",
          "endPoints": ["server-a:1234", "server-b:5678"],
          "allowAdmin": true,
          "connectRetry": 5,
          "connectTimeout": 30,
          "defaultDatabase": 5
        }
      ]
    }

* `id` is used to point to the specific connection in URLs
* Each Redis connection should have at least one end point in `endPoints` array for initial connection, but for clusters with more than one server, you can specify any number of servers in `endPoints`.
* `allowAdmin` is required for some of the Redis commands to be set to `true` (like retrieving configuration or server info). If you're not going to use such commands, it's safer to set `allowAdmin` to `false`.




## URL Scheme

All URLs in Redis module start with `/api/redis`

Except when listing all connections, all URLs start with identification of the target connection for running the commang / query:

`/api/redis/connection/{connectionId}`

The `{connectionId}` is the `id` element specified in configuration file.




# Redis Actions

### List servers
> `GET /api/redis/connection/{connectionId}/servers`
>
> 

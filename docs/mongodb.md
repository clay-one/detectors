[Table of Contents](toc.md)

# Detectors - MongoDB module

The MongoDB module allows you to connect to one (or more) MongoDB servers and query its status and content.

## Configuration

This is the MongoDB part from `connections.json` file:

    "mongodb": {
      "clusters": [
        {
          "id": "cluster-one",
          "connectionString": "mongodb://localhost:27017"
        },
        {
          "id": "cluster-two",
          "connectionString": "mongodb://db1.example.com:27017,db2.example.com:27017"
        },
      ]
    }

* `id` is used to point to the specific cluster in URLs
* Each MongoDB cluster should have a `connectionString` which is a standard [MongoDB Connection String](https://docs.mongodb.com/manual/reference/connection-string/).




## URL Scheme

All URLs in MongoDB module start with `/api/mongodb`

Except when listing all clusters, all URLs start with identification of the target cluster for running the command / query:

`/api/mongodb/cluster/{clusterId}`

The `{clusterId}` is the `id` element specified in configuration file.




# MongoDB Actions

### List clusters
> `GET /api/mongodb/clusters`

### List databases
> `GET /api/mongodb/cluster/{clusterId}/databases`

### Get Replica Set Status
> `GET /api/mongodb/cluster/{clusterId}/replicaset/status`

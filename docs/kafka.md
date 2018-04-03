[Table of Contents](toc.md)

# Detectors - Kafka module

The Kafka module allows you to connect to one (or more) Kafka clusters and query its status and content.

## Configuration

This is the Kafka part from `connections.json` file:

    "kafka": {
      "clusters": [
        {
          "id": "cluster-a",
          "brokers": [
            { "host": "", "port": 1234 },
            { "host": "", "port": 1234 },
            { "host": "", "port": 1234 }
         ]
        },
        {
          "id": "cluster-b",
          "brokers": [
            { "host": "", "port": 1234 },
            { "host": "", "port": 1234 },
            { "host": "", "port": 1234 }
          ]
        }
      ]
    }

* Each Kafka connection is called a **cluster**, which are configured under the `clusters` section
* `id` is used to point to the specific cluster in URLs
* Each Kafka cluster should have at least one **broker** for bootstrap, but for clusters with more than one brokers, you can specify any number of brokers for bootstrapping.




## URL Scheme

All URLs in Kafka module start with `/api/kafka`

Except when listing all clusters, all URLs start with identification of the target cluster for running the commang / query:

`/api/kafka/cluster/{clusterId}`

The `{clusterId}` is the `id` element specified in configuration file.




# Kafka Actions

### Retrieve all cluster metadata
> `GET /api/kafka/cluster/{clusterId}/metadata`
>
> 

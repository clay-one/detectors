# Detectors

[Documentation](docs/toc.md) - [Roadmap](docs/roadmap.md)

Detectors is a set of HTTP-based simple utilities to make monitoring and diagnostics easier for a set of tools (currently
**Kafka** and **Redis**, more are planned for later - see [roadmap](docs/roadmap.md))

Specifically, it can simplify four types of tasks:

* **Monitoring** - Most monitoring tools (such as PRTG, Nagios, ManageEngine, etc.) provide a way to call HTTP endpoints and collect
data, and set alerts and provide historical charts based on them. But many of them lack support for specific tools and software.
*Detectors* provide HTTP endpoints so that these tools can reach into the internals of the software components easily.

* **Simple calculations** - Some figures cannot be directly queried from the tools. Like if you want to watch the rate of change
for a specific paramter, or compare two different figures (eg. calculating the lag on Kafka topics), *Detectors* provide both
module-specific and gereric tools to achive this.

* **Client software independence** - Many of the software tools and databases have proprietary wire
protocols, and require their own client software to connect to them and query the status or data. *Detectors* allows you to
perform simple queries or access diagnostic figures if you have any HTTP client at hand. This can be in everyday jobs, or when
you're away and only have your smart phone with you (and maybe someone calls with bad news)

* **Browsing** - To figure out what's going on inside the software, *Detectors* provide easy HTTP endpoints to show you both
data and internals of the module, like version number, database size, memory usage, or even allow you to directly query
the data in simple predefined ways. This specially comes handy in the emergency when you want to quickly find-out what's wrong.

The project is written in C# and *.NET Core* runtime, so it can be run on any supported environment (Windows, Linux, Mac).





# Modules

Detectors support a few modules now, and more is planned to be added later. Each module has a specific set of HTTP endpoints
available, and more can be added on-demand, based on what is actually needed.

Here's the list of currently available modules and their features:




## Kafka module

* Browse cluster metadata, brokers, topics, partitions, and consumer groups
* See topic partition details, watermark offsets, and commit offsets for a specific consumer group
* Calculate overall (or per-partition) lag for a consumer group on a partition
* Calculate produce / consume rate (change rate of high watermark offsets and commit offsets)

See [Kafka Module Documentation](docs/kafka.md) for configuration and detailed URL schemes.




## Redis module

* Check status of a connection, servers list, ping, echo and server time
* Fetch server configuration, list of connected clients, server info, last save, and slow operations log
* Query database size, random keys, scan keys based on a pattern
* Check key existance, check type of a key, check TTL on a key, or dump the raw contents
* Pub/Sub: list channels, number of subscribers on a channel, and number of pattern-based subscribers
* Redis Lists: query list length, retrieve an item at index, or a range of items
* Redis Sets: query cardinality, check membership, scan members or retrieve all / random members
* Redis Sorted Sets: query cardinality, count by score range, retrieve by score/rank/value range, scan members, query rank/score of a member
* Redis Strings: query string length, fetch whole string or a range, query bits by position, find bit positions, or count bits

See [Redis Module Documentation](docs/redis.md) for configuration and detailed URL schems.




## MongoDB module

MongoDB module is planned but not yet implemented.




## ZooKeeper

ZooKeeper module is planned but not yet implemented.





# Security Considerations

* Detectors does not have any authentication/authorization feature as of now, so the access should be limited via network layer (firewalls, IP filters on web server or reverse proxy, or such)
* NEVER, EVER, expose your Detectors installation on a public network
* Limit network access to Detectors installation in your private network ONLY to the needed clients (eg. monitoring agents)
* Although Detectors does not make any modifications to any of the supported modules, but unauthorized access can give more insight for an intruder to make its way easier toward your systems.
* Although Detectors does not make any modifications to any of the supported modules, but an intruder can use it to impose unwanted load on target systems (eg. KEYS query on Redis) and run DoS (or such) attacks.
* Detectors does not enforce any authorization on the data in target systems, unless enforced by connection credentials by the systems themselves.
* Protect the Detectors installation data well. Configuration can contain sensitive data for accessing target systems. Some commands need administrative priviledge on target systems, so make sure to keep connection information as protected as possible. 

Summary: **Detectors is an Administrative utility. Limit its network access. Handle with extreme caution.**




# Mindsets

There are a few points and concepts that are considered through out the whole project and modules. It's worth knowing them if you're
using Detectors or you want to contribute to it.

* **Simplicity** - All of the functionalities provided by Detectors are extremely simple and small. Implementation is also simple. This makes extendind, adding new functionality, or reading the code very straightforward. So you can very quickly make a fork, add your own logic to it, and create a pull request.

* **Read-only access** - Detectors never provides a way to change anything in the target systems. It's meant to read, browse, query and diagnose. Not to change or configure. (Although reading some data may require administrative priviledges on target system, such as reading configuration settings)

* **Stateful / Stateless** - An instace of Detectos installation does not keep any persistance state. That's why there's no need for a database configuration. It also doesn't keep any state / cache in memory, and builds everything from scratch, per request. The only exception to this rule is the rate calculation, where a set of samples are kept in memory for each figure.

* **Reactive** - An instance of Detectors doesn't do anything on its own actively, unless someone asks for it. It means that there are no backgroud jobs, recurring tasks, scheduled tasks, or any such thing to gather data. It only responds to requests. It doesn't even try to connect to any target systems, unless there's a request for it.

* **No connection pooling** - Connections to the target systems are made for *each request*, and disposed at the end of the request processing. Nothing is kept open. Each request received by Detectors is treated as a whole new world, with its own connections, so that the state of the Detectors instance itself wouldn't affect the monitoring quality and shows a *fresh* view to the target system.

* **Slow by design** - Detectors is *NOT* built for performance, but rather for monitoring. For monitoring purposes, you probably won't need a response in less than 10 milliseconds, rather you'd want a fresh response with the most isolation and the least effect from previous requests or other enviromental factors. So, all the above points mean that the response times are higher that what you'd expect from an operational system, and that's by design.




# General features

The following features apply to all modules:



## Connections

All modules should be configured before use. The configuration for all modules are in the `connections.json` file,
located in the root folder of the web application. Drivers used to connect to target systems are different for
each module, so the connection properties need to be specific to that module. Configuration options for each module is
documented in the module documentation.

Each module supports connecting to multiple target systems, so that you won't need multiple installations / instances
of Detectors to connect to multiple systems. Each connection is identified by an `id` in the configuration file. The `Id`
is only used as a key to identify the target connection, and appears as a part of URL for HTTP calls.

There is no limit on the number of connections each module can have. You can also define multiple connections to the same
server in the configuration (possibly with different connection properties). But you have to make sure `Id` is unique for
each connection of a module. (Duplicate `Id` across different modules is not a problem). Duplicate `Id`s will not be
reported as an error, though. When asking for a connection, the first configuration with that `Id` will be used.




## Result formatting

Server response can be formatted in many different ways, so that you can ask for a format more suitable for your use case and tool. 
For example, some tools may understand JSON formatted results, where others expect XML. Some monitoring tools expect specific
formats in order to be able to parse results and provide alert ranges / charts / historical archives on it.

You can ask Detectors for a specific format by adding an *extension* to the URL (like `.xml` or `.csv`), or provide 
a `format` query string (like `?format=xml` or `?format=csv`).

[Formatting documentation page](docs/formatting.md) provides details about the supported formats.



## Rate calculation

All rate calculations receive an `interval` parameter that specifies the averaging duration. It can be specified as a simple number,
which will be interpreted as the number of minutes, or with `s` / `m` / `h` suffix to specify seconds / minutes / hours.
The returned rate value will always show **change in one minute**, regardless of the specified interval. For example, if you
ask for rate calculation with `10m` as the interval, Detectors will report *change paer minute, averaged in the past ten minutes*.

As detailed in the *Mindsets* section, the Detectors instace is reactive, so it won't go out and sample anything unless it is told
to do so. So keep in mind that if you don't have scheduled jobs (like monitoring agents) to query some rate-related URL, there won't
be enough sampling data available in the memory to perform accurate rate calculations.

Also, as there is no persistace storage involved for rate calculations, all previous samples will be lost when the Detectors instance
is restarted.

So, if you want to query and find out the rate of change for a specific resource, you should either have a regular invocation for
it (such as a monitoring tool or scheduled task), or call it manually multiple times with enough delay, so that samples can be gathered
to calculate a meaningful rate.





# Deployment

For now, there is no binary releases available. So, you'll have to:

1. Make sure you have .NET Core SDK installed on your own machine
1. Make sure you have .NET Core Runtime installed on your production machne
1. Clone the repository
1. Go to src/Root folder
1. Rename `connections.sample.json` to `connections.json`, and edit it (see each module's documentation for configuration)
1. You may want to execute `dotnet run` to test the configuration on your own machine
1. Execute `dotnet publish`
1. Copy the publish folder to your production machine
1. Run Detectors using `dotnet Detectors.Root.dll`

To install Detectors as a service, install behind a reverse proxy (like IIS or Ngnix), or other deployment options, refer to .NET Core documentation specific to your environment.

**Warning**: Make sure you're read and understood the *security considerations* above, before configuring Detectors to access your production environment.






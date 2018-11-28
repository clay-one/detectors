[Table of Contents](toc.md)

# Detectors - Formatting

Available formats:

* **`js`, `json`**: JSON format, pretty-printed for better readability.
* **`xml`**: XML format.
* **`str`**: Convert the result to a string.
* **`brackets`, `brk`**: Convert the result to be a string, and put brackets around the whole result.
* **`csv`**: Comma-separated values. Works well for list results.
* **`dump`, `.dmp`**: Human-readable dump, using `ServiceStack.Text`'s `.Dump()` method.
* **`htm`, `html`**: HTML format for browser rendering. Only works for list results.
* **`jsv`**: JSV format, created by `ServiceStack.Text`.
* **`markdown`, `md`**: MarkDown format. Only works for list results.
* **`table`, `tbl`**: Tabular text format. Only works for list results.
* **`txt`**: Text format. Only works for text-type results.

You can specify format using `.ext` suffix in URL, or using `?format=` in query string.

Examples: 
* `/api/redis/connection/cnn1/servers.table`
* `/api/redis/connection/cnn1/info?format=csv`

If no `format` parameter is specified (not extension nor query string), the result will be rendered using JSON.

Note: Not all format specifiers work with all URLs.

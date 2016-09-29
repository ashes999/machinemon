# MachineMon

Web application for remotely monitoring the state of systems.  Built with ASP.NET, Angular, and RabbitMQ.

# Setup

- Install RabbitMQ
- Add a user to RabbitMQ through the web UI. Grant it access to virtual hosts.
- For clients, add a `config.json` file with the host name, user name, and password to connect to RabbitMQ. 

For localhost-only clients, you can use the default guest credentials. Your `config.json` would look like this:

```json
{
	"hostname": "localhost",
	"username": "guest",
	"password": "guest"
}
```

If your RabbitMQ server is deployed to the host `rabbitserver` and the user name (and password) are `machinemon`, your config would instead look like this:

```json
{
	"hostname": "rabbitserver",
	"username": "machinemon",
	"password": "machinemon"
}
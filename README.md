# MachineMon

Web application for remotely monitoring the state of systems.  Built with ASP.NET, Angular, and RabbitMQ.

# Setup

- Install RabbitMQ
- Add a user to RabbitMQ named `machinemon` with the password `machinemon`. Grant it access to virtual hosts.
- For clients, add a `config.txt` file with a single line: the host name of the RabbitMQ server (eg. `localhost`)
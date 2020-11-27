![Image of UI](/docs/images/demo.gif)

Docker image:

https://hub.docker.com/r/evolvedlight/masstransitui

To run:

To just try it out (data loss ok) run the docker image, providing the following variables:

| Name                                   | Value                                                                                                    | Example              |
|----------------------------------------|----------------------------------------------------------------------------------------------------------|----------------------|
| MassTransitSettings:UserName           | Username for Rabbit AMPQ                                                                                 | test_user            |
| MassTransitSettings:Password           | Password for Rabbit AMPQ                                                                                 | test_password        |
| MassTransitSettings:HostName           | Hostname for Rabbit AMPQ                                                                                 | parrot.rabbitmq.com  |
| MassTransitSettings:VirtualHost        | Virtual host                                                                                             | parrot1              |
| MassTransitSettings:ManagementEndpoint | HTTP endpoint for management                                                                             | https://example.host |
| ConnectionSettings:InternalDatabase    | Path for internal SQLite DB, including filename. If on Kubernetes, ensure that this is on a volume mount | /mnt1/mtdb.db        |

How to develop:

Check out the code

```
dotnet user-secrets init
dotnet user-secrets set "MassTransitSettings:UserName" "YourUserName"
dotnet user-secrets set "MassTransitSettings:Password" "YourPassword"
dotnet user-secrets set "MassTransitSettings:HostName" "sparrow.rmq.cloudamqp.com"
dotnet user-secrets set "MassTransitSettings:VirtualHost" "YourVirtualHost"
dotnet user-secrets set "MassTransitSettings:ManagementEndpoint" "https://example.host"
```
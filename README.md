![Image of UI](/docs/images/demo.gif)

Docker image:

https://hub.docker.com/r/evolvedlight/masstransitui

To run:

You can run the docker image, providing the following environment variables:

| Name                                   | Value                                                                                                    | Example              |
|----------------------------------------|----------------------------------------------------------------------------------------------------------|----------------------|
| MassTransitSettings__UserName           | Username for Rabbit AMPQ                                                                                 | test_user            |
| MassTransitSettings__Password           | Password for Rabbit AMPQ                                                                                 | test_password        |
| MassTransitSettings__HostName           | Hostname for Rabbit AMPQ                                                                                 | parrot.rabbitmq.com  |
| MassTransitSettings__VirtualHost        | Virtual host                                                                                             | parrot1              |
| MassTransitSettings__ManagementEndpoint | HTTP endpoint for management                                                                             | https://example.host |
| ConnectionSettings__InternalDatabase    | Path for internal SQLite DB, including filename. If on Kubernetes, ensure that this is on a volume mount | /mnt1/mtdb.db        |

How to develop:

Check out the code

Install RabbitMq locally

In the console, start the project:
```
dotnet watch --project MassTransitUi\Server\MassTransitUi.Server.csproj
```

To run in docker:
```
docker run -it --add-host=host.docker.internal:host-gateway -p 8080:80 --env ASPNETCORE_ENVIRONMENT=Development --env MassTransitSettings__ManagementEndpoint=http://host.docker.internal:15672 --env MassTransitSettings__HostName=host.docker.internal masstransit:latest
```
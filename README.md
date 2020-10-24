![Image of UI](/docs/images/front.png)

Docker image:

https://hub.docker.com/r/evolvedlight/masstransitui

How to develop:

Check out the code

```
dotnet user-secrets init
dotnet user-secrets set "MassTransitSettings:UserName" "YourUserName"
dotnet user-secrets set "MassTransitSettings:Password" "YourPassword"
dotnet user-secrets set "MassTransitSettings:HostName" "sparrow.rmq.cloudamqp.com"
dotnet user-secrets set "MassTransitSettings:VirtualHost" "YourVirtualHost"
```
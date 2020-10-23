![Image of UI](/docs/images/front.png)

How to develop:

Check out the code

dotnet user-secrets init
dotnet user-secrets set "MassTransitSettings:UserName" "YourUserName"
dotnet user-secrets set "MassTransitSettings:Password" "YourPassword"
dotnet user-secrets set "MassTransitSettings:HostName" "sparrow.rmq.cloudamqp.com"
dotnet user-secrets set "MassTransitSettings:VirtualHost" "YourVirtualHost"
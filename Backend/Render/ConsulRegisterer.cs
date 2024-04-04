using System.Net;
using Consul;

namespace Render;

public static class ConsulRegisterer
{
    
    
  
    public static void Register(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime,
        IConfiguration configuration)
    {
        
        string serviceId = $"ServiceName-{Dns.GetHostName()}-{Guid.NewGuid()}";
        string serviceIp = Environment.GetEnvironmentVariable("POD_IP") ?? configuration.GetValue<string>("POD_IP"); //gets the pod ip from the environment variable {"POD_IP"} injected by kubernetes inside of the docker container passed to the app  or fallback to the appsettings.json
        
        
        var consulClient = new ConsulClient(x => x.Address = new Uri(configuration.GetValue<string>("consulUri")));

        var serviceCheck = new AgentServiceCheck()
        {
            HTTP = $"http://{serviceIp}:{configuration.GetValue<string>("servicePort")}/health", 
            Interval = TimeSpan.FromSeconds(30),
        };
        
        var registration = new AgentServiceRegistration()
        {
            ID = serviceId,
            Name = configuration.GetValue<string>("serviceName"),
            Address = serviceIp,
            Port = configuration.GetValue<int>("servicePort"),
            Checks = [serviceCheck]
        };

        consulClient.Agent.ServiceRegister(registration).Wait();
        lifetime.ApplicationStopping.Register(() => { consulClient.Agent.ServiceDeregister(serviceId).Wait(); });
    }
}
namespace EaiGuy.Email.Sender
{
    using NServiceBus;
    using NServiceBus.Log4Net;
    using NServiceBus.Logging;
    using NServiceBus.Persistence;

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
    {
        public void Customize(BusConfiguration configuration)
        {
            log4net.Config.XmlConfigurator.Configure();
            LogManager.Use<Log4NetFactory>();

            // In NSB 5 you have to use install nuget 'NServiceBus.RavenDB' and use the following line to configure persistence
            configuration.UsePersistence<RavenDBPersistence>();

            var conventionsBuilder = configuration.Conventions();
            conventionsBuilder.DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith("Commands") && t.Namespace.StartsWith("EaiGuy"));
            conventionsBuilder.DefiningEventsAs(t => t.Namespace != null && t.Namespace.EndsWith("Events") && t.Namespace.StartsWith("EaiGuy"));
            conventionsBuilder.DefiningMessagesAs(t => t.Namespace != null && t.Namespace.EndsWith("Messages") && t.Namespace.StartsWith("EaiGuy"));
            //conventionsBuilder.DefiningEncryptedPropertiesAs(p => p.DeclaringType.Namespace != null &&
            //       p.DeclaringType.Namespace.StartsWith("EaiGuy") && p.Name.StartsWith("Encrypted"));
            //conventionsBuilder.DefiningDataBusPropertiesAs(p => p.DeclaringType.Namespace != null &&
            //       p.DeclaringType.Namespace.StartsWith("EaiGuy") && p.Name.StartsWith("DataBus"));
        }
    }
}

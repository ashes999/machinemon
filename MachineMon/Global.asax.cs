using MachineMon.Core.Repositories;
using MachineMon.Repository.Dapper.Repositories;
using MachineMon.Web.RabbitMq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Configuration;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System;
using System.Web.Configuration;
using MachineMon.Core.Domain;
using System.Linq;

namespace MachineMon.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        // Persistent connection to RabbitMQ
        private static IConnection connection;
        private static IModel channel;

        private IGenericRepository repository;

        protected void Application_Start()
        {
            // Boilerplate MVC setup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            this.repository = (IGenericRepository)System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IGenericRepository));
            this.SetupRabbitMqSubscriber();
        }
        
        protected void Application_End()
        {
            channel.Close(200, "Goodbye!");
            connection.Close();
        }

        private void SetupRabbitMqSubscriber()
        {
            // TODO: move this into config: the hostname where RabbitMQ lives
            var factory = new ConnectionFactory() { HostName = "localhost" };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.QueueDeclare(queue: RabbitMqMessageProcessor.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            var processor = new RabbitMqMessageProcessor(repository, HttpContext.Current);

            consumer.Received += (model, eventArgs) =>
            {
                processor.Process(eventArgs);
            };
            channel.BasicConsume(queue: RabbitMqMessageProcessor.QueueName, noAck: true, consumer: consumer);
        }
    }
}

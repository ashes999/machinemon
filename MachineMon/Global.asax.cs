using MachineMon.DataAccess.DataTransferObjects;
using MachineMon.DataAccess.Repositories;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MachineMon
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static IConnection connection;
        private static IModel channel;

        private Repository repository;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            this.repository = new Repository(ConfigurationManager.ConnectionStrings["DefaultConnection"]);
            this.SetupRabbitMqSubscriber();
        }

        protected void Application_End()
        {
            channel.Close(200, "Goodbye!");
            connection.Close();
        }

        private void SetupRabbitMqSubscriber()
        {
            var httpContext = HttpContext.Current;

            var factory = new ConnectionFactory() { HostName = "localhost" };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                try
                {
                    var body = ea.Body;
                    var json = Encoding.UTF8.GetString(body);
                    var message = JsonConvert.DeserializeObject<Message>(json);
                    this.repository.Insert<Message>(message);
                }
                catch (Exception e)
                {
                    // ELMAH doesn't automatically catch these errors. Force it to.
                    Elmah.ErrorLog.GetDefault(httpContext).Log(new Elmah.Error(e));
                }
            };
            channel.BasicConsume(queue: "hello", noAck: true, consumer: consumer);
        }
    }
}

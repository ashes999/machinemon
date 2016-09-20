using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace MachineMon.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Test()
        {
            ViewBag.Messages = new List<string>();

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);

                var ea = consumer.Model.BasicGet("hello", true);
                if (ea != null)
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    ViewBag.Message = message;
                }
                else
                {
                    ViewBag.Message = "(There are no messages in the 'hello' queue.)";
                }

                channel.BasicConsume(queue: "hello",
                                     noAck: true,
                                     consumer: consumer);
            }

            return View();
        }
    }
}
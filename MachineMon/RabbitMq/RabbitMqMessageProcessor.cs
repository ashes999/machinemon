using MachineMon.Core.Domain;
using MachineMon.Core.Repositories;
using MachineMon.Repository.Dapper.Repositories;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Web;

namespace MachineMon.Web.RabbitMq
{
    // TODO: put behind an interface and move to Infrastructure
    public class RabbitMqMessageProcessor
    {
        public const string QueueName = "machinemon";

        private HttpContext httpContext;
        private IGenericRepository repository;
        
        public RabbitMqMessageProcessor(IGenericRepository repository, HttpContext current)
        {
            this.repository = repository;
            this.httpContext = current;
        }

        public void Process(BasicDeliverEventArgs eventArgs)
        {
            try
            {
                var body = eventArgs.Body;
                var json = Encoding.UTF8.GetString(body);
                var message = JsonConvert.DeserializeObject<Message>(json);

                // TODO: keeping history would be nice. It's occassionally useful.                
                this.repository.Execute("DELETE FROM Message WHERE Sender = @sender AND Metric = @metric", new { sender = message.Sender, metric = message.Metric });
                this.repository.Insert<Message>(message);
            }
            catch (Exception e)
            {
                // ELMAH doesn't automatically catch these errors. Force it to.
                Elmah.ErrorLog.GetDefault(httpContext).Log(new Elmah.Error(e));
            }
        }
    }
}
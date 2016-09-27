using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace MachineMon.Controllers
{
    public class RandomController : ApiController
    {
        private static Random random = new Random();

        // GET: api/Random
        public IEnumerable<string> Get()
        {
            var toReturn = new string[] { RandomString(8), RandomString(9), RandomString(13), RandomString(3) };
            return toReturn;
        }

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}

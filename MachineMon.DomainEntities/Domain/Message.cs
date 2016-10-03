using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineMon.Core.Domain
{
    public class Message
    {
        /// <summary>
        /// The machine name of the sender
        /// </summary>
        public string Sender { get; set; } 

        /// <summary>
        /// The metric we're sending data for. The server usually keeps one message per (sender, metric).
        /// </summary>
        public string Metric { get; set; }

        /// <summary>
        /// The actual text message we're sending
        /// </summary>
        public string Contents { get; set; }

        /// <summary>
        /// The date that the message was posted/received, in UTC.
        /// </summary>
        public DateTime MessageDateTimeUtc { get; set; }
    }
}

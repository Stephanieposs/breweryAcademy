using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Behaviours
{
    public class LogMessage
    {
        public string Level { get; set; }
        public string Message { get; set; }
        public string RequestBody { get; set; }
        public string ResponseBody { get; set; }
        public int StatusCode { get; set; }
        public long ElapsedMilliseconds { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}

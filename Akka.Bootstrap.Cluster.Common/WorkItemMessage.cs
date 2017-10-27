using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Akka.Bootstrap.Cluster.Common
{
    public class WorkItemMessage //: IConsistentHashable
    {
        public WorkItemMessage(string message)
        {
            Message = message;
        }

        public string Message { get; }
        
    }
}

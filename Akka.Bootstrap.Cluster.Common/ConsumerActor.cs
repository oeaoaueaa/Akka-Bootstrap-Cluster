using System;
using Akka.Actor;

namespace Akka.Bootstrap.Cluster.Common
{
    public class ConsumerActor : ReceiveActor
    {
        public ConsumerActor()
        {
            Receive<WorkItemMessage>(workItemMessage =>
            {
                Console.WriteLine($"{Self.Path} <- {Sender.Path} ({workItemMessage.Message})");
            });
        }
    }
}
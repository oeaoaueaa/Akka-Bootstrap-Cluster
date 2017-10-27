using System;
using Akka.Actor;

namespace Akka.Bootstrap.Cluster.Common
{
    public class ConsumerActor : ReceiveActor
    {
        private int[] counts = new int[100];

        public ConsumerActor(int index)
        {
            Receive<WorkItemMessage>(workItemMessage =>
            {
                //Console.WriteLine($"{index} <- {Sender.Path} ({workItemMessage.Message})");
                //Console.WriteLine($"{index} <- {workItemMessage.Message}");
                int remoteIndex = int.Parse(workItemMessage.Message);
                counts[remoteIndex]++;
                Console.WriteLine($"({index}) <- ({remoteIndex}) {counts[remoteIndex]}");
            });
            ReceiveAny(m =>
            {
                Console.WriteLine($"{index} UNEXPECTED: {m}");
            });
        }
    }
}
using Akka.Bootstrap.Cluster.Common;
using Akka.Configuration;
using System;
using Akka.Actor;

namespace Akka.Bootstrap.Cluster.Node1
{
    class ConsumerProgram
    {
        static void Main(string[] args)
        {
            var index = args.Length == 1 ? int.Parse(args[0]) : 1;

            var consumerActorSystem = ConsumerClusterActorSystem.CreateActorSystem(index);

            Console.Title = nameof(ConsumerProgram);
            Console.WriteLine($"{nameof(ConsumerClusterActorSystem)} {index}");
            Console.ReadLine();
        }
    }
}

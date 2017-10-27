using System;
using Akka.Actor;
using Akka.Bootstrap.Cluster.Common;
using Akka.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Akka.Bootstrap.Cluster.Node2
{
    class ProgramNode1
    {
        static void Main(string[] args)
        {
            //var builder = new ConfigurationBuilder()
            //    .AddJsonFile("configuration.json", false, true);
            //var configuration = builder.Build();

            
            var index = args.Length == 1 ? int.Parse(args[0]) : 1;

            var actorSystem = ProducerClusterActorSystem.CreateActorSystem(index);

            Console.Title = nameof(ProgramNode1);
            Console.WriteLine($"{nameof(ProducerClusterActorSystem)} {index}");
            Console.ReadLine();
        }
    }
}

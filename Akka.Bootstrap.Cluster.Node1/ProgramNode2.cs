using Akka.Bootstrap.Cluster.Common;
using Akka.Configuration;
using System;
using Akka.Actor;

namespace Akka.Bootstrap.Cluster.Node1
{
    class ProgramNode2
    {
        static void Main(string[] args)
        {
            //var builder = new ConfigurationBuilder()
            //    .AddJsonFile("configuration.json", false, true);
            //var configuration = builder.Build();


            var config = ConfigurationFactory.ParseString(ConfigString);

            var actorSystem = ActorSystem.Create(Constants.ActorSystemName, config);

            actorSystem.ActorOf(PingPongActor.CreateProps(), "ping");

            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }

        private class PingRemote : ReceiveActor
        {
            public PingRemote()
            {
                ReceiveAny(m => { Console.WriteLine(m);});
            }
        }


        private const string ConfigString = @"
akka {
    stdout-loglevel = INFO
    loglevel = INFO
    log-config-on-start = on
    actor {
        deployment {
            /ping/pong {
                router = broadcast-group
                routees.paths = [""/user/ping""]
                cluster {
                    enabled = on
                    max-nr-of-instances-per-node = 1
                    allow-local-routees = off
                    use-role = ""Node2""
                }
            }
        }
        provider = ""Akka.Cluster.ClusterActorRefProvider, Akka.Cluster""
        serializers {
            json = Akka.Serialization.NewtonSoftJsonSerializer
            bytes = Akka.Serialization.ByteArraySerializer
        }
        serialization-bindings {
            ""System.Object"" = json
        }
        debug {
            receive = on 
            autoreceive = on
            lifecycle = on
            event-stream = on
            unhandled = on
        }
    }
    remote {
        log-remote-lifecycle-events = off
	    log-received-messages = off
        helios.tcp {
            transport-class = ""Akka.Remote.Transport.Helios.HeliosTcpTransport, Akka.Remote""
            applied-adapters = []
            transport-protocol = tcp
            hostname = ""127.0.0.1""
            port = 33702
            maximum-frame-size = 12800000b
        }
    }
    cluster {
        seed-nodes = [""akka.tcp://BootstrapCluster@127.0.0.1:33701""]
        roles = [""Node2""]
    }
}
";
    }
}

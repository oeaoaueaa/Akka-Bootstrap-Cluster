using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;
using Akka.Configuration;

namespace Akka.Bootstrap.Cluster.Common
{
    public static class ConsumerClusterActorSystem
    {
        public static ActorSystem CreateActorSystem(int index)
        {
            var configString = ConfigString.Replace("port = 33801", $"port = 338{index:00}");
            var config = ConfigurationFactory.ParseString(configString);

            var actorSystem = ActorSystem.Create(Constants.ActorSystemName, config);

            //actorSystem.ActorOf(PingPongActor.CreateProps(), "ping");

            var consumerActorRef = actorSystem.ActorOf(Props.Create<ConsumerActor>(index), "consumer");

            return actorSystem;
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
                    use-role = ""Producer""
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
            port = 33801
            maximum-frame-size = 12800000b
        }
    }
    cluster {
        seed-nodes = [""akka.tcp://BootstrapCluster@127.0.0.1:33701""]
        roles = [""Consumer""]
    }
}
";
    }
}

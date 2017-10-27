using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;
using Akka.Configuration;

namespace Akka.Bootstrap.Cluster.Common
{
    public static class ProducerClusterActorSystem
    {
        public static ActorSystem CreateActorSystem(int index)
        {
            var configString = ConfigString.Replace("port = 33701", $"port = 337{index:00}");
            var config = ConfigurationFactory.ParseString(configString);

            var actorSystem = ActorSystem.Create(Constants.ActorSystemName, config);

            //actorSystem.ActorOf(PingPongActor.CreateProps(), "ping");
            var producerActorRef = actorSystem.ActorOf(Props.Create<ProducerActor>(index), "producer");


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
                    use-role = ""Consumer""
                }
            }
            /producer/consumeractors {
                #router = broadcast-group
                #router = consistent-hashing-pool
                router = consistent-hashing-group
                virtual-nodes-factor = 16
                routees.paths = [""/user/consumer""]
                cluster {
                    enabled = on
                    max-nr-of-instances-per-node = 1
                    allow-local-routees = off
                    use-role = ""Consumer""
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
            port = 33701
            maximum-frame-size = 12800000b
        }
    }
    cluster {
        seed-nodes = [""akka.tcp://BootstrapCluster@127.0.0.1:33701""]
        roles = [""Producer""]
    }
}
";
    }
}

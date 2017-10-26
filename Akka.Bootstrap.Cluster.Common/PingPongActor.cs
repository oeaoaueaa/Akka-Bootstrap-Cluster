using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;
using Akka.Routing;

namespace Akka.Bootstrap.Cluster.Common
{
    public class PingPongActor : ReceiveActor
    {
        private readonly IActorRef _remotePingActor;

        public PingPongActor()
        {

            _remotePingActor = Context.ActorOf(Props.Empty.WithRouter(FromConfig.Instance), "pong");
            Ready();
        }

        private void Ready()
        {
            Receive<Ping>(ping =>
            {
                Console.WriteLine($"Received ping {ping.Timestamp} from {Context.Sender.Path}");
                Sender.Tell(new Pong(DateTime.UtcNow));
            });
            Receive<Pong>(pong =>
            {
                Console.WriteLine($"Reply pong {pong.Timestamp}");
            });
            Receive<DoPing>(_ =>
            {
                _remotePingActor.Tell(new Ping(DateTime.UtcNow));
            });
        }

        protected override void PreStart()
        {
            Context.System.Scheduler.ScheduleTellRepeatedly(TimeSpan.Zero, TimeSpan.FromSeconds(5), Self,
                DoPing.Instance, Self);
            base.PreStart();
        }

        private class Pong
        {
            public DateTime Timestamp { get; }

            public Pong(DateTime timestamp)
            {
                Timestamp = timestamp;
            }
        }

        private class Ping
        {
            public Ping(DateTime timestamp)
            {
                Timestamp = timestamp;
            }

            public DateTime Timestamp { get; }
        }

        private class DoPing
        {
            public static readonly DoPing Instance = new DoPing();
        }

        public static Props CreateProps()
        {
            return Props.Create<PingPongActor>();
        }
    }
}

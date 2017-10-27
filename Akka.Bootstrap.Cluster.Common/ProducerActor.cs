using System;
using Akka.Actor;
using Akka.Routing;

namespace Akka.Bootstrap.Cluster.Common
{
    public class ProducerActor :ReceiveActor 
    {
        private readonly int _index;
        private readonly IActorRef _consumerActor;

        public ProducerActor(int index)
        {
            _index = index;

            _consumerActor = Context.ActorOf(Props.Empty.WithRouter(FromConfig.Instance), "consumeractors");
            Context.System.Scheduler.ScheduleTellRepeatedly(TimeSpan.FromSeconds(index), TimeSpan.FromSeconds(1), Self, Produce.Instance, Self);

            Receive<Produce>(_ =>
            {
                _consumerActor.Tell(new WorkItemMessage($"{_index}"));
            });
        }


        private class Produce
        {
            public static Produce Instance { get; } = new Produce();
        }
    }
}
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Loyufei.DomainEvents;
using Zenject;

namespace Gomuku
{
    public class SceneProgress : AggregateRoot, IInitializable
    {
        public SceneProgress(DomainEventService service) : base(service)
        {

        }

        public void Initialize() 
        {
            this.SettleEvents(Declarations.Gomuku, new IDomainEvent[]
            {
                new InitScene(),
                new StartScene()
            });
        }
    }

    public class InitScene : DomainEventBase 
    {

    }

    public class StartScene : DomainEventBase 
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei.DomainEvents;

namespace Gomuku
{
    public class Presenter : Loyufei.MVP.Presenter
    {
        public Presenter(DomainEventService service) : base(service)
        {

        }

        public override object GroupId => Declarations.Gomuku;
    }
}
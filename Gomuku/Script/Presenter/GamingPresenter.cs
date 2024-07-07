using System.Collections;
using System.Collections.Generic;
using Loyufei;
using Loyufei.DomainEvents;

namespace Gomuku
{
    public class GamingPresenter : Presenter 
    {
        public GamingPresenter(GamingModel model, DomainEventService service) : base(service)
        {
            Model = model;
        }

        public GamingModel Model       { get; }

        protected override void RegisterEvents()
        {
            DomainEventService.Register<ResetBoard>(Reset, GroupId);
            DomainEventService.Register<StepStone> (Step , GroupId);
        }

        public void Step(StepStone step) 
        {
            var winner = Model.Step(step.StoneType, step.Id);

            if (winner != EStoneType.None)
            {
                SettleEvents(new EndGaming(), new StopTimer());
            }

            else 
            {
                SettleEvents(new SwitchTimer()); 
            }
        }

        public void Reset(ResetBoard reset) 
        {
            Model.Reset();
        }
    }

    public class StepStone : DomainEventBase 
    {
        public StepStone(EStoneType stoneType, int id) 
        {
            StoneType = stoneType;
            Id        = id;
        }

        public EStoneType StoneType { get; }
        public int        Id        { get; }
    }

    public class ResetBoard : DomainEventBase 
    {

    }

    public class EndGaming : DomainEventBase 
    {

    }
}
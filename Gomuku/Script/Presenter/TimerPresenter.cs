using System.Collections;
using System.Collections.Generic;
using Loyufei;
using Loyufei.DomainEvents;

namespace Gomuku
{
    public class TimerPresenter : Presenter
    {
        public TimerPresenter(TimerModel model, DataUpdater dataUpdater, DomainEventService service) : base(service)
        {
            Model       = model;
            DataUpdater = dataUpdater;

            Init();
        }

        public TimerModel  Model       { get; }
        public DataUpdater DataUpdater { get; }

        protected override void RegisterEvents()
        {
            DomainEventService.Register<StartTimer> (Start , GroupId);
            DomainEventService.Register<StopTimer>  (Stop  , GroupId);
            DomainEventService.Register<ResetTimer> (Reset , GroupId);
            DomainEventService.Register<SwitchTimer>(Switch, GroupId);
        }

        public void Init() 
        {
            Model.Reset();
            Model.Timers.Values.ForEach(timer =>
            {
                timer.OnInterval(timer =>
                {
                    DataUpdater.Update(Model.Turn.ToString(), timer.LeftTime);
                });

                timer.OnTimeEnd(timer =>
                {
                    if (timer.TimeMode == ETimeMode.Normal)
                    {
                        timer.TimeMode = ETimeMode.Limit;
                    }

                    SettleEvents(new EndTime());
                });
            });
        }

        public void Start(StartTimer start) 
        {
            Model.Start();
        }

        public void Stop(StopTimer stop) 
        {
            Model.Stop();
        }

        public void Reset(ResetTimer reset) 
        {
            Model.Reset();

            DataUpdater.Update(Declarations.Black, Model.Timers[EStoneType.Black].LeftTime);
            DataUpdater.Update(Declarations.White, Model.Timers[EStoneType.White].LeftTime);
        }

        public void Switch(SwitchTimer switchTimer) 
        {
            Model.Switch();

            var message = (Model.Turn == EStoneType.Black ? "¶Â´Ñ" : "¥Õ´Ñ") + "¦^¦X";

            DataUpdater.Update(Declarations.Message, message);
        }
    }

    public class StartTimer : DomainEventBase 
    {

    }

    public class ResetTimer : DomainEventBase 
    {

    }

    public class StopTimer : DomainEventBase 
    {

    }

    public class SwitchTimer : DomainEventBase 
    {

    }

    public class EndTime : DomainEventBase 
    {

    }
}
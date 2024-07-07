using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.TestTools;
using Zenject;
using Loyufei;
using Loyufei.DomainEvents;
using Gomuku;

[TestFixture]
public class TimerTest : ZenjectUnitTestFixture
{
    [SetUp]
    public void Binding() 
    {
        SignalBusInstaller.Install(Container);

        Container
            .DeclareSignal<IDomainEvent>()
            .WithId(Declarations.Gomuku);

        Container
            .Bind<IDomainEventBus>()
            .To<DomainEventBus>()
            .AsCached()
            .WithArguments(Declarations.Gomuku);

        Container
            .Bind<DomainEventService>()
            .AsSingle();

        Container
            .Bind<TimerModel>()
            .AsSingle();

        Container
            .Bind<TimerPresenter>()
            .AsSingle();

        Container.
            Bind<EndTimeCatcher>()
            .AsSingle();
    }

    [UnityTest]
    public IEnumerator BaseTimerTest()
    {
        var normal   = 10f;
        var limit    = 2f;
        var interval = 0.1f;

        var timer = new Timer(normal, limit, interval)
            .OnTimeEnd (timer => timer.TimeMode = ETimeMode.Limit);

        timer.Start();

        for (; !timer.Pause;) 
        {
            yield return null;
        }
    }

    [UnityTest]
    public IEnumerator ModelTest()
    {
        var endTest = false;
        var model   = Container.Resolve<TimerModel>();

        model.Reset(10f, 2f);
        model.Timers.Values.ForEach(timer =>
        {
            timer.OnTimeEnd(timer =>
            {
                if (timer.TimeMode == ETimeMode.Normal) 
                {
                    timer.TimeMode = ETimeMode.Limit;
                }

                model.Switch();

                endTest = true;
            });
        });

        model.Start();

        var black = model.Timers[EStoneType.Black];
        var white = model.Timers[EStoneType.White];

        Assert.AreEqual(black.Pause, false);
        Assert.AreEqual(white.Pause, true);

        model.Switch();

        Assert.AreEqual(black.Pause, true);
        Assert.AreEqual(white.Pause, false);

        for (; !endTest;) 
        {
            yield return null;
        }

        Assert.AreEqual(black.Pause, false);
        Assert.AreEqual(white.Pause, true);
        
        Assert.AreEqual(white.TimeMode, ETimeMode.Limit);

        model.Stop();
    }

    [UnityTest]
    public IEnumerator PresenterTest() 
    {
        var presenter = Container.Resolve<TimerPresenter>();
        var service   = Container.Resolve<DomainEventService>();
        var endTime   = Container.Resolve<EndTimeCatcher>();
        var model     = presenter.Model;
        var gomuku    = Declarations.Gomuku;

        service.Post(new InitScene(), gomuku);

        model.Reset(10f, 2f);

        var black = model.Timers[EStoneType.Black];
        var white = model.Timers[EStoneType.White];
        
        service.Post(new StartTimer(), gomuku);

        Assert.AreEqual(black.Pause, false);
        Assert.AreEqual(white.Pause, true);

        service.Post(new SwitchTimer(), gomuku);

        Assert.AreEqual(black.Pause, true);
        Assert.AreEqual(white.Pause, false);

        for (; !endTime.EndTest;)
        {
            yield return null;
        }

        Assert.AreEqual(black.Pause, true);
        Assert.AreEqual(white.Pause, true);

        Assert.AreEqual(white.TimeMode, ETimeMode.Limit);

        service.Post(new StopTimer(), gomuku);
    }

    private class EndTimeCatcher : AggregateRoot
    {
        public EndTimeCatcher(DomainEventService service) : base(service)
        {
            service.Register<EndTime>(EndTime, Declarations.Gomuku);
        }

        public bool EndTest { get; protected set; } = false;

        public void EndTime(EndTime end) 
        {
            EndTest = true;
        }
    }
}
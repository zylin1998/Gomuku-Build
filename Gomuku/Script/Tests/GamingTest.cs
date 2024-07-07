using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Zenject;
using NUnit.Framework;
using Loyufei;
using Loyufei.DomainEvents;
using UnityEngine;

namespace Gomuku.UnitTest
{
    [TestFixture]
    public class GamingTest : ZenjectUnitTestFixture
    {
        private int[] _Record = new int[]
        {
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
            0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0,
            0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1,
            2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 2,
            1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
            2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 2, 1, 2, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 2, 1, 2, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        };

        [SetUp]
        public void Binding() 
        {
            SignalBusInstaller.Install(Container);

            Container.
                DeclareSignal<IDomainEvent>().
                WithId(Declarations.Gomuku);

            Container
                .Bind<IDomainEventBus>()
                .To<DomainEventBus>()
                .AsCached()
                .WithArguments(Declarations.Gomuku);

            Container
                .Bind<DomainEventService>()
                .AsSingle();

            Container
                .Bind<Report>()
                .AsSingle();

            Container
                .Bind<Documentary>()
                .AsSingle();

            Container
                .Bind<GamingModel>()
                .AsSingle();

            Container
                .Bind<GamingPresenter>()
                .AsSingle();
        }

        [Test]
        public void DocumentaryTest() 
        {
            var documentary = Container.Resolve<Documentary>();

            Assert.AreEqual(2, documentary.Records.Count);

            documentary.Records.Values.ForEach(record =>
            {
                Assert.AreEqual(Declarations.Size.Pow(2), record.OfType<IEntity>().Count());

                record
                    .OfType<IEntity>()
                    .ForEach(entity => Assert.AreEqual(0, entity.Data));
            });

            Assert.AreEqual( true, documentary.Record(EStoneType.Black, 85));
            Assert.AreEqual(false, documentary.Record(EStoneType.Black, 85));
            Assert.AreEqual( true, documentary.Record(EStoneType.White, 73));
            Assert.AreEqual(    1, documentary.Query (EStoneType.Black, 85));
            Assert.AreEqual(    1, documentary.Query (EStoneType.White, 73));
        }

        [Test]
        public void ModelTest() 
        {
            var model = Container.Resolve<GamingModel>();

            model.Reset();

            model.Documentary.Records.Values.ForEach(record =>
            {
                Assert.AreEqual(Declarations.Size.Pow(2), record.OfType<IEntity>().Count());

                record
                    .OfType<IEntity>()
                    .ForEach(entity => Assert.AreEqual(0, entity.Data));
            });

            var black  = new List<int>();
            var white  = new List<int>();
            var winner = new List<EStoneType>();

            for (int id = 0; id < _Record.Length; id++)
            {
                if (_Record[id] == 1) { black.Add(id); }
                if (_Record[id] == 2) { white.Add(id); }
            }

            var length = Math.Max(black.Count, white.Count);

            for (var index = 0; index < length; index++)
            {
                if (index < black.Count)
                {
                    winner.Add(model.Step(EStoneType.Black, black[index]));

                    Assert.AreEqual(1, model.Documentary.Query(EStoneType.Black, black[index]));
                }

                if (index < white.Count)
                {
                    winner.Add(model.Step(EStoneType.White, white[index]));

                    Assert.AreEqual(1, model.Documentary.Query(EStoneType.White, white[index]));
                }
            }

            Assert.AreEqual(black.Count + white.Count, model.StepCount);
            Assert.AreEqual(28, winner.Count(type => Equals(type, EStoneType.None)));
            Assert.AreEqual(1 , winner.Count(type => Equals(type, EStoneType.Black)));
        }

        [Test]
        public void RecordTest()
        {
            var report    = Container.Resolve<Report>();
            var service   = Container.Resolve<DomainEventService>();
            var presenter = Container.Resolve<GamingPresenter>();

            service.Post(new ResetBoard(), Declarations.Gomuku);
            
            var black = new List<int>();
            var white = new List<int>();

            for (int id = 0; id < _Record.Length; id++) 
            {
                if (_Record[id] == 1) { black.Add(id); }
                if (_Record[id] == 2) { white.Add(id); }
            }

            var length = Math.Max(black.Count, white.Count);

            for(var index = 0; index < length; index++) 
            {
                if (index <black.Count) 
                {
                    service.Post(new StepStone(EStoneType.Black, black[index]), Declarations.Gomuku);
                }

                if (index < white.Count)
                {
                    service.Post(new StepStone(EStoneType.White, white[index]), Declarations.Gomuku);
                }
            }

            Assert.AreEqual(EStoneType.Black, report.Winner);
            Assert.AreEqual(black.Count + white.Count, report.StepCount);
        }
    }
}
using Loyufei;
using Loyufei.DomainEvents;
using UnityEngine;
using Zenject;

namespace Gomuku
{
    public class GomukuInstaller : MonoInstaller
    {
        [SerializeField]
        private GameObject _Stone;

        public override void InstallBindings()
        {
            #region Factory

            Container
                .BindMemoryPool<StoneListener, StonePool>()
                .FromComponentInNewPrefab(_Stone)
                .AsCached();

            #endregion

            #region Data Struction

            Container
                .Bind<Report>()
                .AsSingle();

            Container
                .Bind<Documentary>()
                .AsSingle();

            #endregion

            #region Model

            Container
                .Bind<DataUpdater>()
                .AsSingle();

            Container
                .Bind<GamingModel>()
                .AsSingle();

            Container
                .Bind<TimerModel>()
                .AsSingle();

            #endregion

            #region

            Container
                .Bind<GomukuPresenter>()
                .AsSingle()
                .NonLazy();

            Container
                .Bind<ConfirmPresenter>()
                .AsSingle()
                .NonLazy();

            Container
                .Bind<GamingPresenter>()
                .AsSingle()
                .NonLazy();

            Container
                .Bind<TimerPresenter>()
                .AsSingle()
                .NonLazy();

            #endregion

            #region Event

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

            #endregion
        }
    }
}
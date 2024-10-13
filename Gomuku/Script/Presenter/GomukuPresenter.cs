using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Loyufei;
using Loyufei.DomainEvents;
using System;

namespace Gomuku
{
    public class GomukuPresenter : Presenter
    {
        public GomukuPresenter(GomukuView view, DataUpdater dataUpdater, Report report, DomainEventService service) : base(service)
        {
            View        = view;
            DataUpdater = dataUpdater;
            Report      = report;

            Init();
        }

        public GomukuView View         { get; }
        public DataUpdater DataUpdater { get; }
        public Report      Report      { get; }

        private EStoneType _Turn = EStoneType.None;

        private List<int>           _Records;
        private List<StoneListener> _Stones;
        private bool                _Gaming;
        private OptionListener      _Start;
        private OptionListener      _Quit;

        protected override void RegisterEvents()
        {
            DomainEventService.Register<EndTime>  (OnTimeEnd, GroupId);
            DomainEventService.Register<EndGaming>(EndGaming, GroupId);
        }

        private void Init() 
        {
            var options = View
                .OfType<OptionListener>()
                .ToDictionary(k => k.Id);

            _Start = options[0];
            _Quit  = options[1];

            _Start.AddListener((id => 
            {
                _Records = new int[Declarations.Size.Pow(2)].ToList();

                View.Stones.ForEach(stone =>
                { 
                    stone.Listener.interactable = true;

                    stone.PointerExit();
                });

                DataUpdater.Update(Declarations.Message, "開始對局，由黑棋先手");

                _Turn = EStoneType.Black;

                SettleEvents(new ResetBoard(), new ResetTimer(), new StartTimer());

                _Gaming = true;

                _Start.Listener.interactable = false;
            }));
            _Quit.AddListener((id =>
            {
                var send = new SendConfirm("是否離開遊戲",
                    () => Application.Quit(),
                    () =>
                    {
                        if (_Gaming) SettleEvents(new StartTimer());
                    });

                SettleEvents(new StopTimer(), send);
            }));

            _Stones = View.Stones.ToList();

            _Stones.ForEach(stone =>
            {
                stone.Listener
                    .OnPointerEnterAsObservable()
                    .Subscribe((data) => stone.PointerEnter(_Turn));
                stone.Listener
                    .OnPointerExitAsObservable()
                    .Subscribe((data) => stone.PointerExit());

                stone.AddListener((id) => Step((int)id));
            });
        }

        private void OnTimeEnd(EndTime endTime) 
        {
            var index = -1;
            for(; index <= -1; ) 
            {
                var temp = UnityEngine.Random.Range(0, _Records.Count - 1);

                if (_Records[temp] == 0) { index = temp; }
            }

            Step(index);
        }

        private void EndGaming(EndGaming endGaming) 
        {
            var str = (Report.Winner == EStoneType.Black ? "黑棋" : "白棋") + "獲勝";

            DataUpdater.Update("Message", str);

            _Stones.ForEach(s => s.Listener.interactable = false);

            _Gaming = false;

            _Start.Listener.interactable = true;
        }

        private void Step(int id) 
        {
            SettleEvents(new StepStone(_Turn, id));

            _Records[id] = (int)_Turn;

            _Stones[id].Click(_Turn);

            _Turn = _Turn == EStoneType.Black ? EStoneType.White : EStoneType.Black;
        }
    }
}
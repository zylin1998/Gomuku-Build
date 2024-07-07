using System;
using System.Collections;
using System.Collections.Generic;

namespace Gomuku
{
    public class TimerModel
    {
        public TimerModel() 
        {
            Timers = new()
            {
                { EStoneType.Black, _Black },
                { EStoneType.White, _White },
            };
        }

        private Timer _Black = new();
        private Timer _White = new();

        public Dictionary<EStoneType, Timer> Timers { get; }
        public EStoneType                    Turn   { get; private set; }

        public void Reset() 
        {
            _Black.Reset();
            _White.Reset();
        }

        public void Reset(float normal,float limit)
        {
            _Black.Reset(normal, limit);
            _White.Reset(normal, limit);
        }

        public void Start() 
        {
            Turn = Turn == EStoneType.None ? EStoneType.Black : Turn;

            Timers[Turn].Start();
        }

        public void Stop() 
        {
            _Black.Stop();
            _White.Stop();
        }

        public void Switch() 
        {
            Timers[Turn].Stop();

            Turn = Turn == EStoneType.Black ? EStoneType.White : EStoneType.Black;

            var timer = Timers[Turn];

            if (timer.TimeMode == ETimeMode.Normal)
            {
                timer.Pause = false;
            }

            else { timer.Start(); }
        }
    }
}
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Gomuku
{
    public enum ETimeMode 
    {
        Normal = 0,
        Limit  = 1,
    }

    public class Timer
    {
        public Timer() : this(Declarations.NormalTime, Declarations.LimitTime, Declarations.Interval)
        {
            
        }

        public  Timer(float normal, float limitTime, float interval) 
        {
            NormalTime = normal;
            LimitTime  = limitTime;
            Interval   = interval;

            Reset();

            Observable
                .Interval (TimeSpan.FromSeconds(Interval))
                .Where    (time => !Pause)
                .Subscribe(time => CountTime());
        }

        public float     NormalTime { get; protected set; }
        public float     LimitTime  { get; protected set; }
        public float     Interval   { get; set; }
        public float     LeftTime   { get; protected set; }
        public ETimeMode TimeMode   { get; set; }
        public bool      Pause      { get; set; }

        private Action<Timer> _IntervalCallBack = (timer) => { };
        private Action<Timer> _TimeEndCallBack  = (timer) => { };

        public event Action<Timer> IntervalCallBack
        {
            add    => _IntervalCallBack += value;

            remove => _IntervalCallBack -= value;
        }

        public event Action<Timer> OnTimeEndCallBack
        {
            add    => _TimeEndCallBack += value;

            remove => _TimeEndCallBack -= value;
        }

        public void Reset(float normal, float limit) 
        {
            NormalTime = normal; 
            LimitTime  = limit;

            Reset();
        }

        public void Reset() 
        {
            LeftTime = NormalTime;
            TimeMode = ETimeMode.Normal;
            Pause    = true;
        }

        public void CountTime() 
        {
            var action = LeftTime <= 0 ? _TimeEndCallBack : _IntervalCallBack;

            action?.Invoke(this);

            Pause = LeftTime <= 0;
            
            LeftTime -= Interval;
        }

        public void Start() 
        {
            LeftTime = TimeMode == ETimeMode.Normal ? NormalTime : LimitTime;
            Pause    = false;
        }

        public void Stop() 
        {
            Pause = true;
        }

        public Timer OnInterval(Action<Timer> callback) 
        {
            _IntervalCallBack = callback;

            return this;
        }

        public Timer OnTimeEnd(Action<Timer> callback)
        {
            _TimeEndCallBack = callback;

            return this;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using Loyufei;

namespace Gomuku
{
    public class TimeContext : TextContext
    {
        public override void SetContext(object context)
        {
            var time   = context.To<float>();
            var minute = (int)time / 60;
            var second = (int)time % 60;
            var str    = string.Format("{0}:{1}", minute, second >= 10 ? second : "0" + second);

            base.SetContext(str);
        }
    }
}
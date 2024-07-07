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
            var time = context.To<float>();
            var str  = string.Format("{0}:{1, 2}", (int)time / 60, (int)time % 60f);

            base.SetContext(str);
        }
    }
}
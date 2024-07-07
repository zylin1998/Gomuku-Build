using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gomuku
{
    public enum EStoneType 
    {
        None  = 0,
        Black = 1,
        White = 2,
    }

    public static class Declarations
    {
        public const string Gomuku     = "Gomuku";
        public const string Black      = "Black";
        public const string White      = "White";
        public const string Message    = "Message";
        public const int    Size       = 15;
        public const float  NormalTime = 300f;
        public const float  LimitTime  = 15f;
        public const float  Interval   = 0.02f;
    }
}

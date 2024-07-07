using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Loyufei.ViewManagement;

namespace Gomuku
{
    public class ButtonListener : MonoListenerAdapter<Button>
    {
        public override void AddListener(Action<object> callBack)
        {
            Listener.onClick.AddListener(() => callBack.Invoke(Id));
        }
    }
}